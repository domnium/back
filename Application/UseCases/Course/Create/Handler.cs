using Domain;
using Domain.Entities;
using Domain.Entities.Core;
using Domain.ExtensionsMethods;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Course.Create;

public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IDbCommit _dbCommit;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IIARepository _iaRepository;
    private readonly IMessageQueueService _messageQueueService;
    private readonly ITemporaryStorageService _temporaryStorageService;

    public Handler(ICourseRepository courseRepository, IDbCommit dbCommit,
        ICategoryRepository categoryRepository,
        ITeacherRepository teacherRepository,
        IMessageQueueService messageQueueService,
        IIARepository iaRepository,
        ITemporaryStorageService temporaryStorageService)
    {
        _courseRepository = courseRepository;
        _dbCommit = dbCommit;
        _categoryRepository = categoryRepository;
        _teacherRepository = teacherRepository;
        _messageQueueService = messageQueueService;
        _iaRepository = iaRepository;
        _temporaryStorageService = temporaryStorageService;
    }

    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetWithParametersAsyncWithTracking(c => c.Id == request.CategoryId, cancellationToken);
        if (category is null) return new BaseResponse<object>(404, "Category not found");

        if (await _courseRepository.GetWithParametersAsyncWithTracking(c => c.Name.Name.Equals(request.Name), cancellationToken) is not null)
            return new BaseResponse<object>(409, "Course with this name already exists");

        var teacher = await _teacherRepository.GetWithParametersAsyncWithTracking(t => t.Id == request.TeacherId, cancellationToken);
        if (teacher is null) return new BaseResponse<object>(404, "Teacher not found");

        var ia = await _iaRepository.GetWithParametersAsyncWithTracking(i => i.Id == request.IAId, cancellationToken);
        if (ia is null) return new BaseResponse<object>(404, "IA not found");

        var picture = new Picture(new BigString(Configuration.PicturesCoursesPath),
            false, new AppFile(request.Picture.OpenReadStream(), request.Picture.FileName),
            new BigString(Configuration.PicturesCoursesPath), 
            ContentTypeExtensions.ParseMimeType(request.Picture.ContentType));

        var trailer = new Video(
            new BigString(Configuration.VideoCoursesTrailer), 
            false,
            new VideoFile(request.Trailer!.OpenReadStream(),
            request.Trailer.FileName),
            ContentTypeExtensions.ParseMimeType(request.Trailer.ContentType) 
             );

        if (picture.Notifications.Any() || trailer.Notifications.Any())
            return new BaseResponse<object>(400, "Invalid file", picture.Notifications.Concat(trailer.Notifications).ToList());

        var course = new Domain.Entities.Core.Course(
            new UniqueName(request.Name),
            new Description(request.Description),
            new BigString(request.AboutDescription),
            request.Price,
            request.TotalHours,
            new Url(request.NotionUrl),
            ia,
            trailer,
            category,
            teacher,
            picture
        );

        if (!course.IsValid)
            return new BaseResponse<object>(400, "Invalid course", course.Notifications.ToList());

        await _courseRepository.CreateAsync(course, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        // Salvar arquivos em disco e enfileirar para upload (em paralelo)
        var pictureSaveTask = _temporaryStorageService.SaveAsync(
            Configuration.PicturesCoursesPath,
            picture.Id,
            request.Picture.OpenReadStream(),
            cancellationToken
        );

        var trailerSaveTask = _temporaryStorageService.SaveAsync(
            Configuration.VideoCoursesTrailer,
            trailer.Id,
            request.Trailer.OpenReadStream(),
            cancellationToken
        );

        await Task.WhenAll(pictureSaveTask, trailerSaveTask);

        // Enfileira uploads
        var uploadPictureTask = _messageQueueService.EnqueueUploadMessageAsync(
            new UploadFileMessage(
                picture.Id,
                Configuration.BucketArchives,
                Configuration.PicturesCoursesPath,
                request.Picture.ContentType,
                pictureSaveTask.Result
            ), cancellationToken);

        var uploadTrailerTask = _messageQueueService.EnqueueUploadMessageAsync(
            new UploadFileMessage(
                trailer.Id,
                Configuration.BucketArchives,
                Configuration.VideoCoursesTrailer,
                request.Trailer.ContentType,
                trailerSaveTask.Result
            ), cancellationToken);

        await Task.WhenAll(uploadPictureTask, uploadTrailerTask);

        return new BaseResponse<object>(201, "Course created successfully");
    }
}