using Domain;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.Delete;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IAwsService _awsService;
    private readonly IDbCommit _dbCommit;
    private readonly IPictureRepository _pictureRepository;

    public Handler(IStudentRepository studentRepository,
     IDbCommit dbCommit,
     IPictureRepository pictureRepository,
     IAwsService awsService)
    {
        _studentRepository = studentRepository;
        _dbCommit = dbCommit;
        _pictureRepository = pictureRepository;
        _awsService = awsService;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var studentFound = await _studentRepository
            .GetWithParametersAsync(x => x.Id.Equals(request.StudentId),
             cancellationToken);

        if(studentFound is null) return new BaseResponse(400, "User does not exists");

        var awsSuccessfulDelete = await _awsService.DeleteFileAsync(Configuration.BucketArchives, studentFound.Picture.AwsKey.Body);

        if (!awsSuccessfulDelete) return new BaseResponse(400, "Error deleting file in AWS S3");

        await _pictureRepository.DeleteAsync(studentFound.Picture, cancellationToken);

        await _studentRepository.DeleteAsync(studentFound, cancellationToken);

        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse(200, "Student Deleted");
    }
}
