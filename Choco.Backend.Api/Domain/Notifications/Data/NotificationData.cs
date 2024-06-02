using Choco.Backend.Api.Data.Models;

namespace Choco.Backend.Api.Domain.Notifications.Data;

public class NotificationData
{
    public required Notification Notification { get; set; }
    public required List<Guid> Recipients { get; set; }
}