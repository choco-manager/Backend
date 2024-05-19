using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Sinks.OpenTelemetry;
using Serilog.Templates.Themes;
using SerilogTracing.Expressions;

namespace Api.Extensions;

public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration Configure(this LoggerConfiguration configuration)
    {
        configuration.Enrich.WithExceptionDetails(
                new DestructuringOptionsBuilder()
                    .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() })
            )
            .Enrich.FromLogContext()
            .WriteTo.Console(Formatters.CreateConsoleTextFormatter(theme: TemplateTheme.Code))
            .WriteTo.Seq(
                Environment.GetEnvironmentVariable("SEQ_URL") ?? "localhost:5431",
                apiKey: Environment.GetEnvironmentVariable("SEQ_API_KEY")
            )
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Default", LogEventLevel.Debug)
            .MinimumLevel
            .Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Verbose);

        return configuration;
    }
}