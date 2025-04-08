using Flunt.Notifications;
using Flunt.Validations;

namespace Domain.ValueObjects;

public class PaymentDetails : Notifiable<Notification>
{
    public string PaymentGateway { get; private set; }
    public string TransactionId { get; private set; }

    public PaymentDetails(string gateway, string transactionId)
    {
        PaymentGateway = gateway;
        TransactionId = transactionId;

        AddNotifications(new Contract<Notification>()
            .Requires()
            .IsNotNullOrEmpty(PaymentGateway, "PaymentDetails.PaymentGateway", "Gateway de pagamento é obrigatório")
            .IsNotNullOrEmpty(TransactionId, "PaymentDetails.TransactionId", "ID da transação é obrigatório")
        );
    }
}