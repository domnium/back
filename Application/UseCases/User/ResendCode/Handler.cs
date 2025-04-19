using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Flunt.Notifications;
using Flunt.Validations;
using MediatR;

namespace Application.UseCases.User.ResendCode;

public class Handler(
    IUserRepository userRepository,
    IDbCommit dbCommit,
    IMessageQueueService messageQueueService) : IRequestHandler<Request, BaseResponse>
{
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmail(request.email, cancellationToken);
        var contract = new Contract<Notifiable<Notification>>()
            .Requires()
            .IsNotNull(user, "Email", "Email not registered");

        user?.AddNotifications(contract);
        if (user is null || user.Notifications.Any())
            return new BaseResponse(404, "Request invalid", user?.Notifications?.ToList());

        user.AssignToken(new Random().Next(1000, 9999).ToString());
        var updateTask = Task.Run(() => userRepository.Update(user), cancellationToken);

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

        await Task.WhenAll(updateTask, emailTask);
        await dbCommit.Commit(cancellationToken);
        return new BaseResponse(200, "Code sent successfully");
    }
}
