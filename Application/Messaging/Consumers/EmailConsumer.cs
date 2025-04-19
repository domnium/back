using Domain.Interfaces.Services;
using Domain.Records;
using MassTransit;

namespace Application.Messaging.Consumers;

public class EmailConsumer : IConsumer<EmailMessage>
{
    private readonly IEmailService _emailService;

    public EmailConsumer(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<EmailMessage> context)
    {
        var msg = context.Message;
        await _emailService.SendEmailAsync(
            toName: msg.ToName, 
            toEmail: msg.To,
            subject: msg.Subject,
            body: msg.Body,
            fromName: msg.FromName ?? "Domnum Solutions",
            fromEmail: msg.FromEmail ?? "no-reply@domnum.com",
            cancellationToken: context.CancellationToken
        );
    }
}
