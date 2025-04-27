using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetStudentModuleProgress;

/// <summary>
/// Handler responsável por retornar o progresso de um estudante em um módulo.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly ICourseProgressRepository _courseProgressRepository;

    /// <summary>
    /// Construtor para o handler de progresso do módulo de um estudante.
    /// </summary>
    public Handler(ICourseProgressRepository courseProgressRepository)
    {
        _courseProgressRepository = courseProgressRepository;
    }

    /// <summary>
    /// Manipula o retorno do progresso de um estudante em um módulo.
    /// </summary>
    /// <param name="request">Request contendo os identificadores do estudante e do módulo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var moduleProgress = await _courseProgressRepository.GetModuleProgress(request.StudentId, request.ModuleId);

        if (moduleProgress == 0)
            return new BaseResponse<Response>(404, "Student or module not found");

        var response = new Response(moduleProgress);

        return new BaseResponse<Response>(200, "Module progress retrieved", response);
    }
}