using System;
using Domain;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Student.Create;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICategoryRepository _studentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IPictureRepository _pictureRepository;

    public Handler(ICategoryRepository studentRepository,
     IUserRepository userRepository,
     IDbCommit dbCommit,
     IPictureRepository pictureRepository)
    {
        _studentRepository = studentRepository;
        _userRepository = userRepository;
        _dbCommit = dbCommit;
        _pictureRepository = pictureRepository;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        if(await _userRepository
            .GetWithParametersAsync(x => x.Id.Equals(request.UserId),
             cancellationToken) is null) return new BaseResponse(400, "User does not exists");

        
    }
}
