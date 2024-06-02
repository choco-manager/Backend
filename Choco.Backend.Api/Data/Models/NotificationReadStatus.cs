namespace Choco.Backend.Api.Data.Models;

public class NotificationReadStatus
{
    public required Notification Notification { get; set; }
    public required User User { get; set; }
    public bool IsRead { get; set; }
}