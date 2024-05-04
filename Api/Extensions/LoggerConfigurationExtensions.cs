using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;

namespace Api.Extensions;

public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration Configure(this LoggerConfiguration configuration)
    {
        configuration
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
            .MinimumLevel
            .Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Verbose);

        return configuration;
    }
}