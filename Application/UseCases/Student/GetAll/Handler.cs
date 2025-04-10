using System;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetAll;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IStudentRepository _studentRepository;
    public Handler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var students = await _studentRepository.GetAllProjectedAsync(
            x => x.DeletedDate != null, 
            x => new {
                x.Id,
                x.Name,
                x.Picture.UrlTemp
            }
            ,cancellationToken, 0, 100, x => x.Picture
        );

        if(students is null) return new BaseResponse(404, "Students not found");
        return new BaseResponse(200, "Students found", null, students);
    }
}
