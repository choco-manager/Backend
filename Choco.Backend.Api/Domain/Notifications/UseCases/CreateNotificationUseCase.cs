using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Notifications.Data;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Notifications.UseCases;

public class CreateNotificationUseCase(AppDbContext db) : IUseCase<NotificationData, EmptyResponse>
{
    public async Task<Result<EmptyResponse>> Execute(NotificationData req, CancellationToken ct = default)
    {
        await db.Notifications.AddAsync(req.Notification, ct);

        foreach (var recipient in req.Recipients)
        {
            await db.NotificationsReadStatus.AddAsync(new NotificationReadStatus
            {
                NotificationId = req.Notification.Id,
                RecipientId = recipient,
                IsRead = false
            }, ct);
        }

        await db.SaveChangesAsync(ct);
        
        // TODO: Add firebase calls

        return Result.Success(new EmptyResponse());
    }
}