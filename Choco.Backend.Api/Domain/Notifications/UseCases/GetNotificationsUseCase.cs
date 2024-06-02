using System.Security.Claims;
using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.Notifications.Data;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Notifications.UseCases;

public class GetNotificationsUseCase(AppDbContext db) : IAuthorizedUseCase<EmptyRequest, NotificationsList>
{
    public async Task<Result<NotificationsList>> Execute(ClaimsPrincipal user, EmptyRequest req,
        CancellationToken ct = default)
    {
        var login = user.ClaimValue(ClaimTypes.Name);

        if (login is null)
        {
            return Result.Unauthorized();
        }

        var userData = await db.Users.Where(e => e.Login == login).FirstOrDefaultAsync(ct);

        if (userData is null)
        {
            return Result.NotFound(nameof(login));
        }

        var readNotifications = await db.NotificationsReadStatus
            .Where(e => e.Recipient == userData && e.IsRead)
            .Include(e => e.Notification)
            .OrderByDescending(e => e.Notification.Timestamp)
            .Take(10)
            .Select(e => new NotificationDto
            {
                Title = e.Notification.Title,
                Description = e.Notification.Description,
                Timestamp = e.Notification.Timestamp,
                TriggerId = e.Notification.TriggerId,
                NotificationType = e.Notification.NotificationType,
                IsRead = e.IsRead,
            })
            .ToListAsync(ct);

        var unreadNotifications = db.NotificationsReadStatus
            .Where(e => e.Recipient == userData && !e.IsRead)
            .Include(e => e.Notification);

        List<NotificationDto> notificationDtos = [];
        foreach (var readStatus in unreadNotifications)
        {
            readStatus.IsRead = true;

            notificationDtos.Add(new NotificationDto
            {
                Title = readStatus.Notification.Title,
                Description = readStatus.Notification.Description,
                Timestamp = readStatus.Notification.Timestamp,
                TriggerId = readStatus.Notification.TriggerId,
                NotificationType = readStatus.Notification.NotificationType,
                IsRead = false,
            });
        }

        await db.SaveChangesAsync(ct);


        notificationDtos = notificationDtos
            .OrderByDescending(e => e.Timestamp)
            .ToList();

        var notifications = notificationDtos
            .Concat(readNotifications)
            .ToList();

        return Result.Success(
            new NotificationsList
            {
                Notifications = notifications,
            }
        );
    }
}