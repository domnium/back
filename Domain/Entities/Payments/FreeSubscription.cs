using Domain.Entities.Abstracts;
using Domain.ValueObjects;

namespace Domain.Entities.Payments;

public class FreeSubscription : Subscription
{
    public FreeSubscription(Guid studentId, SubscriptionPeriod period)
        : base(studentId, period)
    {
    }
}