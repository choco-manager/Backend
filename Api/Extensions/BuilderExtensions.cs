using System.Reflection;
using Api.Common;
using Api.Configuration;
using Api.Configuration.Swagger;
using Api.Data;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            }, options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        var token = ctx.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");

                        if (false) // TODO: write some stuff to check if token was not revoked
                        {
                            ctx.Fail("Token Revoked!");
                        }
                    }
                };
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
        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes());

        var registrations = types.Where(type => !type.IsAbstract)
            .Where(type => !type.IsGenericTypeDefinition)
            .Select(type => new
            {
                type,
                services = type.GetInterfaces()
                    .Where(iface => iface.IsGenericType)
                    .Where(iface => iface.GetGenericTypeDefinition() == typeof(IUseCase<,>))
            })
            .SelectMany(t => t.services, (t, service) => new { service, t.type });

        foreach (var reg in registrations)
        {
            builder.Services.AddTransient(reg.service, reg.type);
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