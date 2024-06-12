using System.Text;
using Choco.Backend.Api.Common.Processors;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Notifications.Data;
using Choco.Backend.Api.Domain.Notifications.UseCases;
using FastEndpoints;
using FastEndpoints.Swagger;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Order = FastEndpoints.Order;

namespace Choco.Backend.Api.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication ConfigureAuthorization(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static WebApplication ConfigureFastEndpoints(this WebApplication app)
    {
        app.UseFastEndpoints(opts =>
        {
            opts.Versioning.Prefix = "v";
            opts.Versioning.PrependToRoute = true;
            opts.Endpoints.ShortNames = true;
            opts.Endpoints.Configurator = ep =>
            {
                ep.DontAutoSendResponse();
                ep.PostProcessor<HandleExceptionsPostProcessor>(Order.Before);
                ep.PostProcessor<ConvertResultToStatusCodePostProcessor>(Order.After);
            };
        });

        return app;
    }

    public static WebApplication ConfigureSwaggerGen(this WebApplication app)
    {
        app.UseSwaggerGen(opts =>
            {
                var docPrefix = app.Environment.IsDevelopment() ? "" : "/api";
                var docPath = "/swagger/{documentName}/swagger.json";

                var sb = new StringBuilder();
                sb.Append(docPrefix);
                sb.Append(docPath);

                opts.Path = sb.ToString();
            },
            uiConfig: opts =>
            {
                opts.ServerUrl = app.Environment.IsDevelopment()
                    ? ""
                    : app.Configuration.GetRequiredSection("BaseUrl").Value;
            }
        );

        return app;
    }

    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AppDbContext>();

        Log.Logger.Information("Checking if has pending migrations...");

        var pendingMigrations = context.Database.GetPendingMigrations().ToList();
        if (pendingMigrations.Count == 0)
        {
            return app;
        }

        Log.Logger.Information(
            "Found pending migrations: {PendingMigrations}, migrating...",
            pendingMigrations);
        context.Database.Migrate();

        return app;
    }

    public static async Task<WebApplication> RegisterRecurringTasks(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        var createNotificationUseCase = scope.ServiceProvider.GetRequiredService<CreateNotificationUseCase>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var recipients = await db.Users.Select(e => e.Id).ToListAsync();

        var notification = new NotificationData
        {
            Notification = new Notification
            {
                Title = "Пора провести ревизию!",
                NotificationType = NotificationType.DoStocktake,
                Timestamp = DateTime.UtcNow,
                TriggerId = Guid.Empty
            },
            Recipients = recipients
        };

        jobManager.AddOrUpdate("do-stocktaking-notification",
            () => createNotificationUseCase.Execute(notification, default),
            "30 15 20 * *"
        );

        return app;
    }
}