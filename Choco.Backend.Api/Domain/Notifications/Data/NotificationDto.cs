using Choco.Backend.Api.Data.Enums;

namespace Choco.Backend.Api.Domain.Notifications.Data;

public class NotificationDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Timestamp { get; set; }
    public NotificationType NotificationType { get; set; }
    public Guid TriggerId { get; set; }
    public bool IsRead { get; set; }
}