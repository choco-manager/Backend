using FastEndpoints;
using FastEndpoints.Security;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry(opts => {
        opts.Endpoint = "http://localhost:4317";
        opts.Protocol = OtlpProtocol.Grpc;
        opts.ResourceAttributes = new Dictionary<string, object> {
            ["service.name"] = "ChocoManager Main API",
        };
    })
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Default", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Verbose)
    .CreateLogger();

var bld = WebApplication.CreateBuilder();
bld.Services
    .AddAuthenticationJwtBearer(s => { s.SigningKey = ""; })
    .AddAuthorization()
    .AddFastEndpoints();

var app = bld.Build();
app
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints();
app.Run();