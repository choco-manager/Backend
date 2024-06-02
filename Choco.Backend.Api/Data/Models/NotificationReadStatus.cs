namespace Choco.Backend.Api.Data.Models;

public class NotificationReadStatus
{
    public Notification Notification { get; set; }
    public Guid NotificationId { get; set; }
    public User Recipient { get; set; }
    public Guid RecipientId { get; set; }
    public bool IsRead { get; set; }
}