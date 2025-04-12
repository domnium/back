using Domain.Interfaces;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetById;

/// <summary>
/// Handler responsável por retornar apenas um estudante.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IStudentRepository _studentRepository;

    /// <summary>
    /// Construtor para o handler de retorno de apenas um estudante.
    /// </summary>
    public Handler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    /// <summary>
    /// Manipula o retorno de apenas um estudante com base no identificador encaminhado.
    /// </summary>
    /// <param name="request">Request com o identificador do estudante para filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetProjectedAsync(
            x => x.DeletedDate != null && x.Id == request.StudentId, 
            x => new {
                x.Id,
                x.Name,
                x.Picture.UrlTemp
            }
            ,cancellationToken, x => x.Picture
        );

        if(student is null) return new BaseResponse(404, "Student not found");
        return new BaseResponse(200, "Student found", null, student);
    }
}
