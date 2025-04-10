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
    private readonly IAwsService _awsService;
    private readonly IDbCommit _dbCommit;
    private readonly IPictureRepository _pictureRepository;

    public Handler(IMessageQueueService messageQueueService,
     IStudentRepository studentRepository,
     IUserRepository userRepository,
     IDbCommit dbCommit,
     IPictureRepository pictureRepository,
     IAwsService awsService)
    {
        _messageQueueService = messageQueueService;
        _studentRepository = studentRepository;
        _userRepository = userRepository;
        _dbCommit = dbCommit;
        _pictureRepository = pictureRepository;
        _awsService = awsService;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var userFound = await _userRepository
            .GetWithParametersAsync(x => x.Id.Equals(request.UserId),
             cancellationToken);

        if(userFound is null) return new BaseResponse(400, "User does not exists");

        var pictureId = Guid.NewGuid();

        var awsKey = await _awsService.UploadFileAsync(
            Configuration.BucketArchives,
            pictureId.ToString(),
            request.Picture.OpenReadStream(),
            request.Picture.ContentType,
            cancellationToken
        );

        if(awsKey is null) return new BaseResponse(400, "Error uploading file to AWS S3");

        var newPicture = new Picture(new BigString(awsKey), true, new AppFile(request.Picture.OpenReadStream(), "FotoEstudante"));
        newPicture.SetGuid(pictureId);
        newPicture.SetBucket(Configuration.BucketArchives);
        newPicture.SetTemporaryUrl(new Url(awsKey), DateTime.UtcNow.AddDays(1));

        if (newPicture is null || newPicture.Notifications.Any()) return new BaseResponse(400, "Error creating picture"
            + newPicture.Notifications.Select(x => x.Message).ToString());

        var storedPicture = await _pictureRepository.CreateReturnEntity(newPicture, cancellationToken);

        var newStudent = new Domain.Entities.Core.Student(
            new UniqueName(request.Name),
            userFound,
            request.IsFreeStudent,
            newPicture
        );

        if(newStudent.Notifications.Any())
            return new BaseResponse(400, "Invalid student", newStudent.Notifications.ToList(), null);

        await _studentRepository.CreateAsync(newStudent, cancellationToken);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse(201, "Student Created");
    }
}
