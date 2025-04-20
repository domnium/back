using Domain;
using Domain.Entities;
using Domain.ExtensionsMethods;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Teacher.Create;

/// <summary>
/// Handler responsável pela criação de um novo estudante,
/// com associação a um usuário existente, imagem de perfil e envio assíncrono de upload para RabbitMQ.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IMessageQueueService _messageQueueService;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IDbCommit _dbCommit;
    private readonly ITemporaryStorageService _temporaryStorageService;

    /// <summary>
    /// Construtor para o handler de criação de professores.
    /// </summary>
    public Handler(
        IMessageQueueService messageQueueService,
        ITeacherRepository teacherRepository,
        IDbCommit dbCommit,
        ITemporaryStorageService temporaryStorageService)
    {
        _messageQueueService = messageQueueService;
        _teacherRepository = teacherRepository;
        _dbCommit = dbCommit;
        _temporaryStorageService = temporaryStorageService;
    }

    /// <summary>
    /// Manipula a criação de um novo professor, salvando a imagem em disco e enfileirando-a para upload.
    /// </summary>
    /// <param name="request">Request com dados do professor e imagem</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        // Cria a imagem
        var picture = new Picture(null, false, new AppFile(request.Picture.OpenReadStream(), request.Picture.FileName),
            new BigString(Configuration.PicturesTeacherPath),
            ContentTypeExtensions.ParseMimeType(request.Picture.ContentType)
        );

        if (picture.Notifications.Any())
            return new BaseResponse(400, "Error creating picture", picture.Notifications.ToList());

        // Cria entidade Teacher com imagem
        var newTeacher = new Domain.Entities.Core.Teacher(
            new UniqueName(request.Name),
            new Email(request.Email),
            new Cpf(request.Cpf),
            request.Phone,
            new BigString(request.Endereco),
            request.Cep,
            !string.IsNullOrEmpty(request.Tiktok) ? new Url(request.Tiktok) : null,
            !string.IsNullOrEmpty(request.Instagram) ? new Url(request.Instagram) : null,
            !string.IsNullOrEmpty(request.GitHub) ? new Url(request.GitHub) : null,
            new Description(request.Description),
            picture
        );

        if (newTeacher.Notifications.Any())
            return new BaseResponse(400, "Invalid teacher", newTeacher.Notifications.ToList());

        var teacherAlreadyExists = await _teacherRepository.GetWithParametersAsync(
            t => t.Cpf.Numero.Equals(request.Cpf), cancellationToken
        );

        if(teacherAlreadyExists is not null)
            return new BaseResponse(400, "Teacher Already Exists");

        // Persiste tudo: Teacher + Picture
        await _teacherRepository.CreateAsync(newTeacher, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        // Salva imagem local
        var tempPath = await _temporaryStorageService.SaveAsync(
            Configuration.PicturesTeacherPath,
            picture.Id,
            request.Picture.OpenReadStream(),
            cancellationToken
        );

        // Enfileira imagem para upload
        await _messageQueueService.EnqueueUploadMessageAsync(new UploadFileMessage(
            picture.Id,
            Configuration.BucketArchives,
            Configuration.PicturesTeacherPath,
            request.Picture.ContentType,
            tempPath
        ), cancellationToken);
        return new BaseResponse(201, "Teacher created");
    }
}
