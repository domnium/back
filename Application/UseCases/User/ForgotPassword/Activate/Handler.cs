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
        var userFromDb = await _userRepository.GetWithParametersAsync(u => u.TokenActivate.Equals(request.token),
             cancellationToken);

        if (userFromDb is null) return new BaseResponse<object>(statusCode: 400, message: "Invalid or expired token");

        userFromDb.UpdatePassword(new Password(request.newPassword));
        if(!userFromDb.IsValid)
            return new BaseResponse<object>(statusCode: 400, message: "Invalid password", notifications: userFromDb.Notifications.ToList());

        userFromDb.ClearToken(); 
        _userRepository.Update(userFromDb);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse<object>(statusCode: 200, message: "Password reset successful");
    }
}