﻿using System.Reflection;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.Auth.UseCases;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Choco.Backend.Api.Extensions;

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
                        var useCase = ctx.HttpContext.Resolve<ValidateTokenUseCase>();

                        if ((await useCase.Execute(token)).Value)
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

        builder.Services.Configure<DadataConfiguration>(builder.Configuration.GetRequiredSection("Dadata"));
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<DadataConfiguration>>().Value);

        return builder;
    }

    public static WebApplicationBuilder AddHangfire(this WebApplicationBuilder builder)
    {
        builder.Services.AddHangfire(x =>
        {
            x.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(o =>
                {
                    o.UseNpgsqlConnection(() => builder.Configuration.GetConnectionString("Hangfire"));
                });
        });

        builder.Services.AddHangfireServer(x => x.SchedulePollingInterval = TimeSpan.FromSeconds(30));

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
                    .Where(iface =>
                        iface.GetGenericTypeDefinition() == typeof(IUseCase<,>) ||
                        iface.GetGenericTypeDefinition() == typeof(IPagedUseCase<,>) ||
                        iface.GetGenericTypeDefinition() == typeof(IAuthorizedUseCase<,>)
                    )
            })
            .SelectMany(t => t.services, (t, service) => new { service, t.type });

        foreach (var reg in registrations)
        {
            builder.Services.AddTransient(reg.type);
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