using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.User.ForgotPassword.Activate;

/// <summary>
/// Handler responsável por ativar a redefinição de senha de um usuário.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IUserRepository _userRepository;
    private readonly IDbCommit _dbCommit;

    /// <summary>
    /// Construtor do handler de ativação de redefinição de senha.
    /// </summary>
    public Handler(IUserRepository userRepository, IDbCommit dbCommit)
    {
        _userRepository = userRepository;
        _dbCommit = dbCommit;
    }

    /// <summary>
    /// Manipula a ativação da redefinição de senha.
    /// </summary>
    /// <param name="request">Request contendo o e-mail, token e nova senha</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca o usuário pelo e-mail
        var userFromDb = await _userRepository.GetByEmail(request.email, cancellationToken);
        if (userFromDb == null || !userFromDb.TokenActivate.Equals(request.token))
        {
            return new BaseResponse<object>(
                statusCode: 404,
                message: "User not found or invalid token"
            );
        }

        // Atualiza a senha do usuário
        userFromDb.UpdatePassword(new Password(request.newPassword));
        if (userFromDb.Notifications.Any())
        {
            return new BaseResponse<object>(
                statusCode: 400,
                message: "Request invalid",
                notifications: userFromDb.Notifications.ToList()
            );
        }

        // Atualiza o usuário no repositório
        _userRepository.Update(userFromDb);

        // Confirma as alterações no banco de dados
        await _dbCommit.Commit(cancellationToken);

        // Retorna sucesso
        return new BaseResponse<object>(
            statusCode: 200,
            message: "Password changed successfully"
        );
    }
}