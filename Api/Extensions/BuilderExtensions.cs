using System.Reflection;
using Api.Common;
using Api.Configuration;
using Api.Configuration.Swagger;
using Api.Data;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureFastEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthenticationJwtBearer(s =>
            {
                s.SigningKey = builder.Configuration
                    .GetRequiredSection("Security")
                    .GetRequiredSection("SigningKey")
                    .Value;
            })
            .AddAuthorization()
            .AddFastEndpoints();

        return builder;
    }

    public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContextPool<AppDbContext>(
            opts => opts.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
        );

        return builder;
    }

    public static WebApplicationBuilder MapConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<SecurityConfiguration>(builder.Configuration.GetRequiredSection("Security"));
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<SecurityConfiguration>>().Value);

        return builder;
    }

    public static WebApplicationBuilder AddUseCases(this WebApplicationBuilder builder)
    {
        var types = Assembly.GetCallingAssembly().GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IUseCase<,>)));

        foreach (var type in types)
        {
            builder.Services.AddTransient(typeof(IUseCase<,>), type);
        }

        return builder;
    }

    public static WebApplicationBuilder ConfigureSwaggerDocument(this WebApplicationBuilder builder)
    {
        builder.Services.SwaggerDocument(opts =>
        {
            opts.AutoTagPathSegmentIndex = 0;
            opts.MinEndpointVersion = 3;
            opts.MaxEndpointVersion = 3;
            opts.ShortSchemaNames = true;
            opts.ShowDeprecatedOps = true;
            opts.TagDescriptions = FindTags;
            opts.DocumentSettings = s =>
            {
                s.Title = "ChocoManager Main API";
                s.Description = "Main API of ChocoManager project";
                s.DocumentName = "main-v3";
                s.Version = "v3";
            };
        });

        return builder;
    }

    private static void FindTags(Dictionary<string, string> tags)
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
    }
}