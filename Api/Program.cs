using Api.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Configure()
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