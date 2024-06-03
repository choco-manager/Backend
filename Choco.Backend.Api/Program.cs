using Choco.Backend.Api.Extensions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Serilog;
using SerilogTracing;

Log.Logger = new LoggerConfiguration()
    .Configure()
    .CreateLogger();

using var listener = new ActivityListenerConfiguration()
    .Instrument.AspNetCoreRequests()
    .TraceToSharedLogger();

try
{
    var builder = WebApplication.CreateBuilder();

    builder.Host.UseSerilog();

    builder
        .ConfigureFastEndpoints()
        .ConfigureDatabase()
        .ConfigureSwaggerDocument()
        .MapConfiguration()
        .AddHangfire()
        .AddUseCases();

    var app = builder.Build();

    app
        .ConfigureAuthorization()
        .ConfigureFastEndpoints()
        .ConfigureSwaggerGen()
        .MigrateDatabase()
        .UseHangfireDashboard()
        .UseSerilogRequestLogging();

    await app.RegisterRecurringTasks();

    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "firebase.json")),
    });

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}