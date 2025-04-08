using Domain.Entities;
using Domain.Entities.Abstracts;
using Flunt.Validations;

namespace Domain.Entities.Payments;

public class StripeWebhookEvent : Entity
{
    public string EventId { get; private set; }
    public string Type { get; private set; }
    public string PayloadJson { get; private set; }
    public DateTime ReceivedAt { get; private set; }

    public StripeWebhookEvent(string eventId, string type, string payloadJson)
    {
        EventId = eventId;
        Type = type;
        PayloadJson = payloadJson;
        ReceivedAt = DateTime.UtcNow;

        AddNotifications(new Contract<StripeWebhookEvent>()
            .IsNotNullOrWhiteSpace(EventId, "EventId", "ID do evento é obrigatório")
            .IsNotNullOrWhiteSpace(Type, "Type", "Tipo do evento é obrigatório")
        );
        SetValuesCreate();
    }
}
