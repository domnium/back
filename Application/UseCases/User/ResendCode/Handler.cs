using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Flunt.Notifications;
using Flunt.Validations;
using MediatR;

namespace Application.UseCases.User.ResendCode;

/// <summary>
/// Handler responsável por reenviar o código de ativação para o e-mail do usuário.
/// </summary>
public class Handler(
    IUserRepository userRepository,
    IDbCommit dbCommit,
    IMessageQueueService messageQueueService) : IRequestHandler<Request, BaseResponse<object>>
{
    /// <summary>
    /// Manipula o reenvio do código de ativação.
    /// </summary>
    /// <param name="request">Request contendo o e-mail do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca o usuário pelo e-mail
        var user = await userRepository.GetByEmail(request.email, cancellationToken);

        // Valida se o usuário existe
        var contract = new Contract<Notifiable<Notification>>()
            .Requires()
            .IsNotNull(user, "Email", "Email not registered");

        user?.AddNotifications(contract);

        if (user is null || user.Notifications.Any())
        {
            return new BaseResponse<object>(
                statusCode: 404,
                message: "Request invalid",
                notifications: user?.Notifications?.ToList()
            );
        }

        // Gera um novo token de ativação
        user.AssignToken(new Random().Next(1000, 9999).ToString());

        // Atualiza o usuário no repositório
        var updateTask = Task.Run(() => userRepository.Update(user), cancellationToken);

        // Envia o e-mail com o código de ativação
        var emailTask = messageQueueService.EnqueueEmailMessageAsync(
            new EmailMessage(
                To: user.Email.Address!,
                ToName: user.FullName.FirstName!,
                Subject: "Reenvio de Código de Ativação",
                Body: $"<strong> Seu código de Ativação da Conta: {user.TokenActivate} </strong>",
                IsHtml: true,
                FromName: "Domnum Solutions",
                FromEmail: Configuration.SmtpUser
            ),
            cancellationToken
        );

        // Aguarda a conclusão das tarefas
        await Task.WhenAll(updateTask, emailTask);

        // Confirma as alterações no banco de dados
        await dbCommit.Commit(cancellationToken);

        // Retorna sucesso
        return new BaseResponse<object>(
            statusCode: 200,
            message: "Code sent successfully"
        );
    }
}