namespace Domain.Records;
public record EmailMessage(
    string To,
    string ToName,
    string Subject,
    string Body,
    bool IsHtml = true,
    string? FromName = null,
    string? FromEmail = null
);