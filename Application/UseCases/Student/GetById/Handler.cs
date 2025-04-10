using Domain.Interfaces;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetById;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IStudentRepository _studentRepository;
    public Handler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetAllProjectedAsync(
            x => x.DeletedDate != null && x.Id == request.StudentId, 
            x => new {
                x.Id,
                x.Name,
                x.Picture.UrlTemp
            }
            ,cancellationToken, 0, 1, x => x.Picture
        );

        if(student is null) return new BaseResponse(404, "Student not found");
        return new BaseResponse(200, "Student found", null, student[0]);
    }
}
