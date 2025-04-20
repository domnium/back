using Domain;
using Domain.Entities;
using Domain.ExtensionsMethods;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Student.Create;

/// <summary>
/// Handler responsável pela criação de um novo estudante,
/// com associação a um usuário existente, imagem de perfil e envio assíncrono de upload para RabbitMQ.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IMessageQueueService _messageQueueService;
    private readonly IStudentRepository _studentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDbCommit _dbCommit;
    private readonly ITemporaryStorageService _temporaryStorageService;

    /// <summary>
    /// Construtor para o handler de criação de estudantes.
    /// </summary>
    public Handler(
        IMessageQueueService messageQueueService,
        IStudentRepository studentRepository,
        IUserRepository userRepository,
        IDbCommit dbCommit,
        ITemporaryStorageService temporaryStorageService)
    {
        _messageQueueService = messageQueueService;
        _studentRepository = studentRepository;
        _userRepository = userRepository;
        _dbCommit = dbCommit;
        _temporaryStorageService = temporaryStorageService;
    }

    /// <summary>
    /// Manipula a criação de um novo estudante, salvando a imagem em disco e enfileirando-a para upload.
    /// </summary>
    /// <param name="request">Request com dados do aluno e imagem</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var userFound = await _userRepository
            .GetWithParametersAsync(x => x.Id == request.UserId, cancellationToken);

        if (userFound is null)
            return new BaseResponse(400, "User does not exist");

        // Cria a imagem
        var picture = new Picture(
            null, 
            false, 
            new AppFile(request.Picture.OpenReadStream(), request.Picture.FileName),
            new BigString(Configuration.PicturesStudensPath),
            ContentTypeExtensions.ParseMimeType(request.Picture.ContentType)
            );

        if (picture.Notifications.Any())
            return new BaseResponse(400, "Error creating picture", picture.Notifications.ToList());

        // Cria entidade Student com imagem
        var newStudent = new Domain.Entities.Core.Student(
            new UniqueName(request.Name),
            userFound,
            request.IsFreeStudent,
            picture
        );

        if (newStudent.Notifications.Any())
            return new BaseResponse(400, "Invalid student", newStudent.Notifications.ToList());

        // Persiste tudo: Student + Picture
        await _studentRepository.CreateAsync(newStudent, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        // Salva imagem local
        var tempPath = await _temporaryStorageService.SaveAsync(
            Configuration.PicturesStudensPath,
            picture.Id,
            request.Picture.OpenReadStream(),
            cancellationToken
        );

        // Enfileira imagem para upload
        await _messageQueueService.EnqueueUploadMessageAsync(new UploadFileMessage(
            picture.Id,
            Configuration.BucketArchives,
            Configuration.PicturesStudensPath,
            request.Picture.ContentType,
            tempPath
        ), cancellationToken);

        return new BaseResponse(201, "Student created");
    }
}
