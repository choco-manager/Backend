using Choco.Backend.Api.Extensions;
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
        .AddUseCases();

    var app = builder.Build();

    app
        .ConfigureAuthorization()
        .ConfigureFastEndpoints()
        .ConfigureSwaggerGen()
        .MigrateDatabase()
        .UseSerilogRequestLogging();

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