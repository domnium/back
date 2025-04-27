using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Module.Create;

/// <summary>
/// Handler responsável pela criação de um novo módulo,
/// com associação a um curso existente.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IModuleRepository _moduleRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IDbCommit _dbCommit;

    /// <summary>
    /// Construtor para o handler de criação de módulos.
    /// </summary>
    public Handler(
        IModuleRepository moduleRepository,
        ICourseRepository courseRepository,
        IDbCommit dbCommit)
    {
        _moduleRepository = moduleRepository;
        _courseRepository = courseRepository;
        _dbCommit = dbCommit;
    }

    /// <summary>
    /// Manipula a criação de um novo módulo.
    /// </summary>
    /// <param name="request">Request com dados do módulo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse{object}"/> com status e mensagem</returns>
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Verifica se o curso existe
        var courseFound = await _courseRepository
            .GetWithParametersAsync(x => x.Id == request.CourseId, cancellationToken);

        if (courseFound is null)
            return new BaseResponse<object>(400, "Course does not exist");

        // Anexa o curso ao contexto
        _courseRepository.Attach(courseFound);

        // Cria a entidade Module associada ao curso
        var newModule = new Domain.Entities.Core.Module(
            new UniqueName(request.Name!),
            new Description(request.Description!),
            courseFound
        );

        // Verifica se o módulo é válido
        if (newModule.Notifications.Any())
            return new BaseResponse<object>(400, "Invalid module", newModule.Notifications.ToList());

        // Persiste o módulo no banco de dados
        await _moduleRepository.CreateAsync(newModule, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        // Retorna sucesso
        return new BaseResponse<object>(201, "Module created");
    }
}