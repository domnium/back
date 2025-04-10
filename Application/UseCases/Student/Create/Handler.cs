using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Student.Create;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IMessageQueueService _messageQueueService;
    private readonly IStudentRepository _studentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IPictureRepository _pictureRepository;

    public Handler(IMessageQueueService messageQueueService,
     IStudentRepository studentRepository,
     IUserRepository userRepository,
     IDbCommit dbCommit,
     IPictureRepository pictureRepository)
    {
        _messageQueueService = messageQueueService;
        _studentRepository = studentRepository;
        _userRepository = userRepository;
        _dbCommit = dbCommit;
        _pictureRepository = pictureRepository;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var userFound = await _userRepository
            .GetWithParametersAsync(x => x.Id.Equals(request.UserId),
             cancellationToken);

        if(userFound is null) return new BaseResponse(400, "User does not exists");

        var newPicture = new Picture(null, false, new AppFile(request.Picture.OpenReadStream(),
             request.Picture.FileName));

        if (newPicture.Notifications.Any()) return new BaseResponse(400, "Error creating picture"
            + newPicture.Notifications.Select(x => x.Message).ToString());

        //Crio StoredPicture no banco.
        var storedPicture = await _pictureRepository.CreateReturnEntity(newPicture, cancellationToken);

        var newStudent = new Domain.Entities.Core.Student(
            new UniqueName(request.Name),
            userFound,
            request.IsFreeStudent,
            newPicture
        );

        if(newStudent.Notifications.Any())
            return new BaseResponse(400, "Invalid student", newStudent.Notifications.ToList(), null);

        //Cria Estudante no banco
        await _studentRepository.CreateAsync(newStudent, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        //Salva arquivo no sistema de arquivos local
        var tempPicturePath = Path.Combine(Configuration.PicturesCoursesPath, $"_{storedPicture.Id.ToString()}.{Path
            .GetExtension(request.Picture.FileName)}");

        //Manda mensagem para o RabbitMQ
        await _messageQueueService.EnqueueUploadMessageAsync(new UploadFileMessage
        (
            Id: storedPicture.Id,
            Bucket: Configuration.BucketArchives,
            Path: $"{tempPicturePath}.{Path.GetExtension(request.Picture.FileName)}",
            ContentType: request.Picture.ContentType,
            TempFilePath: tempPicturePath
        ), cancellationToken
        );
        return new BaseResponse(201, "Student Created");
    }
}
