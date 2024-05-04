using System.Reflection;
using Api.Configuration;
using Api.Configuration.Swagger;
using Api.Data;
using Api.Extensions;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        opts.AutoTagPathSegmentIndex = 0;
        opts.MinEndpointVersion = 3;
        opts.MaxEndpointVersion = 3;
        opts.ShortSchemaNames = true;
        opts.ShowDeprecatedOps = true;
        opts.TagDescriptions = tags =>
        {
            var fields = typeof(SwaggerTags).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                var descriptionAttribute = field.GetCustomAttribute<TagDescriptionAttribute>();
                if (descriptionAttribute != null)
                {
                    tags.Add(field.GetValue(null)?.ToString()!, descriptionAttribute.Description);
                }
            }
        };
        opts.DocumentSettings = s =>
        {
            s.Title = "ChocoManager Main API";
            s.Description = "Main API of ChocoManager project";
            s.DocumentName = "main-v3";
            s.Version = "v3";
        };
    })
    .AddUseCases()
    .AddDbContextPool<AppDbContext>(
        opts => opts.UseNpgsql(bld.Configuration.GetConnectionString("Default"))
    )
    .Configure<SecurityConfiguration>(bld.Configuration.GetRequiredSection("Security"))
    .AddSingleton(resolver =>
        resolver.GetRequiredService<IOptions<SecurityConfiguration>>().Value);

var app = bld.Build();
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