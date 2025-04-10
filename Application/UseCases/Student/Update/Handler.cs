using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Student.Update;

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
        var studentFound = await _studentRepository
            .GetWithParametersAsync(x => x.Id.Equals(request.StudentId),
             cancellationToken);

        if(studentFound is null) return new BaseResponse(400, "Student does not exists");

        return new BaseResponse(200, "Student Updated");
    }
}
