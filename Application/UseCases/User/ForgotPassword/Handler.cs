using System;
using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.ForgotPassword;

/// <summary>
/// Handler responsável por gerar um token de redefinição de senha e enviar um e-mail de ativação.
/// </summary>
public class Handler(
    IUserRepository userRepository,
    IDbCommit dbCommit,
    IEmailService emailService,
    IMessageQueueService messageQueueService) : IRequestHandler<Request, BaseResponse<object>>
{
    /// <summary>
    /// Manipula a geração do token de redefinição de senha e o envio do e-mail de ativação.
    /// </summary>
    /// <param name="request">Request contendo o e-mail do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca o usuário pelo e-mail
        var userFromDb = await userRepository.GetByEmail(request.Email, cancellationToken);
        if (userFromDb is null)
        {
            return new BaseResponse<object>(
                statusCode: 404,
                message: "User not found"
            );
        }

        // Gera o token de redefinição de senha
        userFromDb.GenerateToken();
        userRepository.Update(userFromDb);

        var resetLink = $"{Configuration.FrontendUrl}/reset-password/?token={userFromDb.TokenActivate}";

        var commitTask = dbCommit.Commit(cancellationToken);
        var emailTask = messageQueueService.EnqueueEmailMessageAsync(
            new EmailMessage(
                To: userFromDb.Email.Address!,
                ToName: userFromDb.FullName.FirstName,
                Subject: "Altere sua senha!",
                Body: $"<strong> Clique no link para alterar sua senha: <a href=\"{resetLink}\">{resetLink}</a> </strong>",
                IsHtml: true,
                FromName: "Domnum",
                FromEmail: Configuration.SmtpUser
            ),
            cancellationToken
        );

        await Task.WhenAll(commitTask, emailTask);

        // Retorna sucesso
        return new BaseResponse<object>(
            statusCode: 201,
            message: "Password change activation email sent"
        );
    }
}