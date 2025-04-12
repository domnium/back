using Domain;
using Domain.Entities;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Course.Create;

/// <summary>
/// Handler responsável pela criação de um novo curso.
/// O processo inclui validações de entidades relacionadas,
/// armazenamento temporário de arquivos e envio assíncrono de mensagens para upload.
/// </summary>
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
    private readonly ITemporaryStorageService _temporaryStorageService;

    /// <summary>
    /// Construtor do handler de criação de curso.
    /// </summary>
    public Handler(ICourseRepository courseRepository, IDbCommit dbCommit,
        ICategoryRepository categoryRepository,
        ITeacherRepository teacherRepository,
        IMessageQueueService messageQueueService,
        IPictureRepository pictureRepository,
        IVideoRepository videoRepository,
        IParameterRepository parameterRepository,
        IIARepository iaRepository,
        ITemporaryStorageService temporaryStorageService)
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
        _temporaryStorageService = temporaryStorageService;
    }

    /// <summary>
    /// Cria um novo curso com todos os dados associados (categoria, IA, professor, etc.).
    /// </summary>
    /// <param name="request">Dados do curso a ser criado.</param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem.</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetWithParametersAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (category is null) return new BaseResponse(404, "Category not found");

        if (await _courseRepository.GetWithParametersAsync(c => c.Name.Equals(request.Name), cancellationToken) is not null)
            return new BaseResponse(409, "Course with this name already exists");

        var teacher = await _teacherRepository.GetWithParametersAsync(t => t.Id == request.TeacherId, cancellationToken);
        if (teacher is null) return new BaseResponse(404, "Teacher not found");

        var parameter = await _parameterRepository.GetWithParametersAsync(p => p.Id == request.ParametersId, cancellationToken);
        if (parameter is null) return new BaseResponse(404, "Parameter not found");

        var ia = await _iaRepository.GetWithParametersAsync(i => i.Id == request.IAId, cancellationToken);
        if (ia is null) return new BaseResponse(404, "IA not found");

        var picture = new Picture(null, false, new AppFile(request.Image.OpenReadStream(), request.Image.FileName));
        var trailer = new Video(null, false, new VideoFile(request.Trailer.OpenReadStream(), request.Trailer.FileName));

        if (picture.Notifications.Any() || trailer.Notifications.Any())
            return new BaseResponse(400, "Invalid file", picture.Notifications.Concat(trailer.Notifications).ToList());

        var storedPicture = await _pictureRepository.CreateReturnEntity(picture, cancellationToken);
        var storedTrailer = await _videoRepository.CreateReturnEntity(trailer, cancellationToken);

        var course = new Domain.Entities.Core.Course(
            new UniqueName(request.Name),
            new Description(request.Description),
            new BigString(request.AboutDescription),
            request.Price,
            request.TotalHours,
            new Url(request.NotionUrl),
            ia,
            storedTrailer,
            parameter,
            category,
            teacher,
            storedPicture
        );

        if (!course.IsValid)
            return new BaseResponse(400, "Invalid course", course.Notifications.ToList());

        await _courseRepository.CreateAsync(course, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        // Salvar arquivos em disco e enfileirar para upload (em paralelo)
        var pictureSaveTask = _temporaryStorageService.SaveAsync(
            Configuration.PicturesCoursesPath,
            storedPicture.Id,
            request.Image.OpenReadStream(),
            cancellationToken
        );

        var trailerSaveTask = _temporaryStorageService.SaveAsync(
            Configuration.VideoCoursesTrailer,
            storedTrailer.Id,
            request.Trailer.OpenReadStream(),
            cancellationToken
        );

        await Task.WhenAll(pictureSaveTask, trailerSaveTask);

        // Enfileira uploads
        var uploadPictureTask = _messageQueueService.EnqueueUploadMessageAsync(
            new UploadFileMessage(
                storedPicture.Id,
                Configuration.BucketArchives,
                Configuration.PicturesCoursesPath,
                request.Image.ContentType,
                pictureSaveTask.Result
            ), cancellationToken);

        var uploadTrailerTask = _messageQueueService.EnqueueUploadMessageAsync(
            new UploadFileMessage(
                storedTrailer.Id,
                Configuration.BucketArchives,
                Configuration.VideoCoursesTrailer,
                request.Trailer.ContentType,
                trailerSaveTask.Result
            ), cancellationToken);

        await Task.WhenAll(uploadPictureTask, uploadTrailerTask);
        return new BaseResponse(201, "Course created", null, course);
    }
}
