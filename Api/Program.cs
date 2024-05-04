using Api.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry(opts =>
    {
        opts.Endpoint = "http://localhost:4317";
        opts.Protocol = OtlpProtocol.Grpc;
        opts.ResourceAttributes = new Dictionary<string, object>
        {
            ["service.name"] = "ChocoManager Main API",
        };
    })
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Default", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Verbose)
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
    .UseSerilogRequestLogging()
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(opts =>
    {
        opts.Versioning.Prefix = "v";
        opts.Versioning.PrependToRoute = true;
        opts.Endpoints.ShortNames = true;
    })
    .UseSwaggerGen(opts => { opts.Path = "/swagger/{documentName}/swagger.json"; });
app.Run();