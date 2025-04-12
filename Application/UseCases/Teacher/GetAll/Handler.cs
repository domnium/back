using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Teacher.GetAll;

/// <summary>
/// Handler responsável por retornar até 100 professores do sistema.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ITeacherRepository _teacherRepository;

    /// <summary>
    /// Construtor para o handler de retorno de até 100 professores do sistema.
    /// </summary>
    public Handler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    /// <summary>
    /// Manipula o retorno de até 100 professores.
    /// </summary>
    /// <param name="request">Request vazio para o retorno de até 100 professores do sistema</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var teachers = await _teacherRepository.GetAllProjectedAsync(
            x => x.DeletedDate != null, 
            x => new {
                x.Id,
                x.Name,
                x.Tiktok,
                x.Instagram,
                x.GitHub,
                x.Description,
                x.Picture.UrlTemp
            }
            ,cancellationToken, 0, 100, x => x.Picture
        );

        if(teachers is null) return new BaseResponse(404, "Teachers not found");
        return new BaseResponse(200, "Teachers found", null, teachers);
    }
}
