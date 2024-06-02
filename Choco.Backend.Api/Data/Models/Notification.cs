using Choco.Backend.Api.Data.Common;
using Choco.Backend.Api.Data.Enums;

namespace Choco.Backend.Api.Data.Models;

public class Notification : BaseModel
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public NotificationType NotificationType { get; set; }
    public Guid TriggerId { get; set; }
    public DateTime Timestamp { get; set; }
}