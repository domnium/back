using System;
using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Flunt.Notifications;
using Flunt.Validations;
using MediatR;

namespace Application.UseCases.User.Register;

public class Handler(
    IUserRepository userRepository,
    IDbCommit dbCommit,
    IMapper mapper,
    IMessageQueueService messageQueueService) : IRequestHandler<Request, Response>
{
    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<Domain.Entities.Core.User>(request);

        user.AddNotifications(
            new Contract<Notifiable<Notification>>()
                .Requires()
                .IsFalse(await userRepository.GetByEmail(request.Email, cancellationToken) != null, "Email", "Email already registered")
        );

        if (user.Notifications.Any())
            return new Response(404, "Request invalid", user.Notifications.ToList());

        var activationLink = $"{Configuration.PublicUrlFrontEnd}/auth/activate-account?email={Uri.EscapeDataString(user.Email.Address!)
            }&token={Uri.EscapeDataString(user.TokenActivate.ToString()!)}";

        var emailMessage = new EmailMessage(
            To: user.Email.Address!,
            ToName: user.FullName.FirstName!,
            Subject: "Ative sua Conta!",
            Body: $"<strong> Clique no link para ativar sua conta: <a href='{activationLink}'>Ativar Conta</a> </strong>",
            IsHtml: true,
            FromName: "Domnum Solutions",
            FromEmail: Configuration.SmtpUser
        );

        var createTask = userRepository.CreateAsync(user, cancellationToken);
        var emailTask = messageQueueService.EnqueueEmailMessageAsync(emailMessage, cancellationToken);

        await Task.WhenAll(createTask, emailTask);
        await dbCommit.Commit(cancellationToken);

        return mapper.Map<Response>(user);
    }
}
