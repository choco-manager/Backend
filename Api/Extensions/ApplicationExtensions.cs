using Api.Common.Processors;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace Api.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication ConfigureAuthorization(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static WebApplication ConfigureFastEndpoints(this WebApplication app)
    {
        app.UseFastEndpoints(opts =>
        {
            opts.Versioning.Prefix = "v";
            opts.Versioning.PrependToRoute = true;
            opts.Endpoints.ShortNames = true;
            opts.Endpoints.Configurator = ep =>
            {
                ep.DontAutoSendResponse();
                ep.PostProcessor<HandleExceptionsPostProcessor>(Order.Before);
                ep.PostProcessor<ConvertResultToStatusCodePostProcessor>(Order.After);
            };
        });

        return app;
    }

    public static WebApplication ConfigureSwaggerGen(this WebApplication app)
    {
        app.UseSwaggerGen(opts => { opts.Path = "/swagger/{documentName}/swagger.json"; });

        return app;
    }
}