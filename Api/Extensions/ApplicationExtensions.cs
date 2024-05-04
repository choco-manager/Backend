using Api.Common.Processors;
using Api.Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Api.Extensions;

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
        app.UseSwaggerGen(opts => { opts.Path = "/swagger/{documentName}/swagger.json"; });

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
}