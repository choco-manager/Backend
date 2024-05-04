using Api.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Configure()
    .CreateLogger();

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