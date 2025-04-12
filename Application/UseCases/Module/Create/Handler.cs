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
public class Handler : IRequestHandler<Request, BaseResponse>
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
        IDbCommit dbCommit,
        ITemporaryStorageService temporaryStorageService)
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
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var courseFound = await _courseRepository
            .GetWithParametersAsync(x => x.Id == request.CourseId, cancellationToken);

        if (courseFound is null)
            return new BaseResponse(400, "Course does not exist");

        // Cria entidade Module com curso
        var newModule = new Domain.Entities.Core.Module(
            new UniqueName(request.Name),
            new Description(request.Description),
            courseFound
        );

        if (newModule.Notifications.Any())
            return new BaseResponse(400, "Invalid module", newModule.Notifications.ToList());

        // Persiste tudo: Module
        await _moduleRepository.CreateAsync(newModule, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        return new BaseResponse(201, "Module created");
    }
}
