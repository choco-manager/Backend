using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Notifications.Data;
using FastEndpoints;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using Notification = FirebaseAdmin.Messaging.Notification;

namespace Choco.Backend.Api.Domain.Notifications.UseCases;

public class CreateNotificationUseCase(AppDbContext db) : IUseCase<NotificationData, EmptyResponse>
{
    public async Task<Result<EmptyResponse>> Execute(NotificationData req, CancellationToken ct = default)
    {
        List<Message> messages = [];
        await db.Notifications.AddAsync(req.Notification, ct);

        foreach (var recipient in req.Recipients)
        {
            await db.NotificationsReadStatus.AddAsync(new NotificationReadStatus
            {
                NotificationId = req.Notification.Id,
                RecipientId = recipient,
                IsRead = false
            }, ct);

            var tokens = await db.FcmTokens
                .Where(e => e.UserId == recipient)
                .Select(e => e.Token)
                .ToListAsync(ct);

            foreach (var token in tokens)
            {
                messages.Add(new Message
                {
                    Notification = new Notification
                    {
                        Title = req.Notification.Title,
                        Body = req.Notification.Description
                    },
                    Token = token
                });
            }
        }

        await db.SaveChangesAsync(ct);

        await FirebaseMessaging.DefaultInstance.SendEachAsync(messages, ct);

        return Result.Success(new EmptyResponse());
    }
}