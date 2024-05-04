using Api.Data;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
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

var bld = WebApplication.CreateBuilder();
bld.Host.UseSerilog();
bld.Services
    .AddAuthenticationJwtBearer(s =>
    {
        s.SigningKey = bld.Configuration
            .GetRequiredSection("Security")
            .GetRequiredSection("SigningKey")
            .Value;
    })
    .AddAuthorization()
    .AddFastEndpoints()
    .SwaggerDocument(opts =>
    {
        opts.DocumentSettings = s =>
        {
            s.Title = "ChocoManager Main API";
            s.Version = "v3";
        };
    })
    .AddDbContextPool<AppDbContext>(
        opts => opts.UseNpgsql(bld.Configuration.GetConnectionString("Default"))
    );

var app = bld.Build();
app
    .UseSerilogRequestLogging()
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints()
    .UseSwaggerGen();
app.Run();