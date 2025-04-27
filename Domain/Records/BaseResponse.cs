using System.Text.Json.Serialization;
using Flunt.Notifications;

namespace Domain.Records;

public record BaseResponse<T>
{
    public int StatusCode { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Message { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<Notification>? Notifications { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Response { get; init; }

    public BaseResponse(int statusCode, string message, T? response = default, List<Notification>? notifications = null)
    {
        StatusCode = statusCode;
        Message = message;
        Notifications = notifications;
        Response = response;
    }
}