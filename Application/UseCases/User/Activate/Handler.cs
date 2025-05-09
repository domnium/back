using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.Activate;

/// <summary>
/// Handler responsável por ativar um usuário.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IUserRepository _userRepository;
    private readonly IDbCommit _dbCommit;

    /// <summary>
    /// Construtor para o handler de ativação de usuário.
    /// </summary>
    public Handler(IUserRepository userRepository, IDbCommit dbCommit)
    {
        _userRepository = userRepository;
        _dbCommit = dbCommit;
    }

    /// <summary>
    /// Manipula a ativação de um usuário.
    /// </summary>
    /// <param name="request">Request contendo o e-mail e o token do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        if (request.token == Guid.Empty)
            return new BaseResponse<object>(400, "Token inválido");

        // Tenta ativar o usuário
        var user = await _userRepository.ActivateUserAsync(request.email, request.token, cancellationToken);
        if (user is null)
            return new BaseResponse<object>(400, "Usuário ou token inválido");

        // Confirma a ativação
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse<object>(200, "Usuário ativado com sucesso!");
    }
}