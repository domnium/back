using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Teacher.GetById;

/// <summary>
/// Handler responsável por retornar apenas um professor.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ITeacherRepository _teacherRepository;

    /// <summary>
    /// Construtor para o handler de retorno de apenas um professor.
    /// </summary>
    public Handler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    /// <summary>
    /// Manipula o retorno de apenas um professor com base no identificador encaminhado.
    /// </summary>
    /// <param name="request">Request com o identificador de professor para filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetProjectedAsync(
            x => x.DeletedDate != null && x.Id == request.TeacherId, 
            x => new {
                x.Id,
                x.Name,
                x.Picture.UrlTemp
            }
            ,cancellationToken, x => x.Picture
        );

        if(teacher is null) return new BaseResponse(404, "Teacher not found");
        return new BaseResponse(200, "Teacher found", null, teacher);
    }
}
