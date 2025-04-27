using Domain.Records;
using Flunt.Notifications;

namespace Application.UseCases.User.Register;

public record Response(int StatusCode, string? Message = null, List<Notification>? Notifications = null, string? Token = null) 
    : BaseResponse<string>(StatusCode, Message, Token, Notifications);