using System;
using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.User.ForgotPassword;

public class Handler(
    IUserRepository userRepository,
    IDbCommit dbCommit,
    IEmailService emailService,
    IMessageQueueService messageQueueService) : IRequestHandler<Request, BaseResponse>
{
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var userFromDb = await userRepository.GetByEmail(request.Email, cancellationToken);
        if (userFromDb is null)
            return new BaseResponse(404, "User not found");

        userFromDb.GenerateToken();
        userRepository.Update(userFromDb);

        var commitTask = dbCommit.Commit(cancellationToken);
        var emailTask = messageQueueService.EnqueueEmailMessageAsync(
            new EmailMessage(
                To: userFromDb.Email.Address!,
                ToName: userFromDb.FullName.FirstName,
                Subject: "Altere sua senha!",
                Body: $"<strong> Seu código de Alteração de senha: {userFromDb.TokenActivate} </strong>",
                IsHtml: true,
                FromName: "Domnum",
                FromEmail: Configuration.SmtpUser
            ),
            cancellationToken
        );

        await Task.WhenAll(commitTask, emailTask);
        return new BaseResponse(201, "Password change activation email sent");
    }
}
