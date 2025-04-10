using Domain;
using Domain.Entities;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Course.Create;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IDbCommit _dbCommit;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IParameterRepository _parameterRepository;
    private readonly IPictureRepository _pictureRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IIARepository _iaRepository;
    private readonly IMessageQueueService _messageQueueService;

    public Handler(ICourseRepository courseRepository, IDbCommit dbCommit,
        ICategoryRepository categoryRepository,
        ITeacherRepository teacherRepository,
        IMessageQueueService messageQueueService,
        IPictureRepository pictureRepository,
        IVideoRepository videoRepository,
        IParameterRepository parameterRepository,
        IIARepository iaRepository)
    {
        _courseRepository = courseRepository;
        _pictureRepository = pictureRepository;
        _videoRepository = videoRepository;
        _parameterRepository = parameterRepository;
        _dbCommit = dbCommit;
        _categoryRepository = categoryRepository;
        _teacherRepository = teacherRepository;
        _messageQueueService = messageQueueService;
        _iaRepository = iaRepository;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetWithParametersAsync(
            c => c.Id.Equals(request.CategoryId), cancellationToken);

        if (category is null)
            return new BaseResponse(404, "Category not found");

        var courseExist = await _courseRepository.GetWithParametersAsync(
            c => c.Name.Equals(request.Name), cancellationToken);

        if (courseExist is not null)
            return new BaseResponse(409, "Course with this name already exists");

        var teacher = await _teacherRepository.GetWithParametersAsync(
            t => t.Id.Equals(request.TeacherId), cancellationToken);

        if (teacher is null)
            return new BaseResponse(404, "Teacher not found");

        var parameter = await _parameterRepository.GetWithParametersAsync(
            p => p.Id.Equals(request.ParametersId), cancellationToken);

        if (parameter is null)
            return new BaseResponse(404, "Parameter not found");

        var ia = await _iaRepository.GetWithParametersAsync(
            i => i.Id.Equals(request.IAId), cancellationToken);

        if (ia is null)
            return new BaseResponse(404, "IA not found"); 

        var picture = new Picture(null, false, new AppFile(request.Image.OpenReadStream(), request.Image.FileName));
        var trailer = new Video(null, false, new VideoFile(request.Trailer.OpenReadStream(), request.Trailer.FileName));

        if(trailer.Notifications.Any() || picture.Notifications.Any())
            return new BaseResponse(400, "Invalid file", 
                picture.Notifications.Concat(trailer.Notifications).ToList(), 
                null);

        var storedPicture = await _pictureRepository.CreateReturnEntity(picture, cancellationToken);
        var storedTrailer = await _videoRepository.CreateReturnEntity(trailer, cancellationToken);

        // 3. Cria o curso com as referências já salvas
        var course = new Domain.Entities.Core.Course(
            new UniqueName(request.Name),
            new Description(request.Description),
            new BigString(request.AboutDescription),
            request.Price,
            request.TotalHours,
            new Url(request.NotionUrl),
            ia: ia,
            trailer: storedTrailer,
            parameters: parameter,
            category,
            teacher,
            image: storedPicture
        );

        if(course.Notifications.Any())
            return new BaseResponse(400, "Invalid course", course.Notifications.ToList(), null);

        await _courseRepository.CreateAsync(course, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        // 4. Salva arquivos fisicamente após persistência
        var tempPicturePath = Path.Combine(Configuration.PicturesCoursesPath, $"_{storedPicture.Id.ToString()}.{Path
            .GetExtension(request.Image.FileName)}");

        var tempTrailerPath = Path.Combine(Configuration.VideoCoursesTrailer, $"_{storedTrailer.Id.ToString()}.{Path
            .GetExtension(request.Trailer.FileName)}");

        using (var pictureStream = new FileStream(tempPicturePath, FileMode.Create))
            await request.Image.CopyToAsync(pictureStream, cancellationToken);

        using (var trailerStream = new FileStream(tempTrailerPath, FileMode.Create))
            await request.Trailer.CopyToAsync(trailerStream, cancellationToken);

        // 5. Dispara mensagem de upload para cada arquivo
        await _messageQueueService.EnqueueUploadMessageAsync(new UploadFileMessage
        (
            Id: storedPicture.Id,
            Bucket: Configuration.BucketArchives,
            Path: $"{tempPicturePath}.{Path.GetExtension(request.Image.FileName)}",
            ContentType: request.Image.ContentType,
            TempFilePath: tempPicturePath
        ), cancellationToken
        );

        await _messageQueueService.EnqueueUploadMessageAsync(new UploadFileMessage
        (
            Id: storedTrailer.Id,
            Bucket: Configuration.BucketArchives,
            Path: $"{tempPicturePath}.{Path.GetExtension(request.Trailer.FileName)}",
            ContentType: request.Trailer.ContentType,
            TempFilePath: tempTrailerPath
        ), cancellationToken
        );

        return new BaseResponse(201, "Course created", null, course);
    }
}
