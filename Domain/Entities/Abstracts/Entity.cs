using Flunt.Notifications;

namespace Domain.Entities.Abstracts;

public abstract class Entity : Notifiable<Notification>
{
    public Guid Id { get; protected set; }
    public DateTime? CreatedDate { get;  private set; }
    public DateTime? UpdatedDate { get;  private set; }
    public DateTime? DeletedDate { get;  private set; }
    
    protected void AddNotificationsFromValueObjects(params List<Notifiable<Notification?>?>? valueObjects)
    {
        foreach (var valueObject in valueObjects)
        {
            AddNotifications(valueObject.Notifications);
        }
    }
    public void SetValuesUpdate() => UpdatedDate = DateTime.Now;
    public void SetValuesDelete() => DeletedDate = DateTime.Now;
    public void SetValuesCreate() => CreatedDate = UpdatedDate = DateTime.Now;
    
}