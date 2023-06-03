using Backend.Data;
using Backend.Modules;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
  .MinimumLevel.Override("Default", LogEventLevel.Debug)
  .CreateLogger();

builder.Host.UseSerilog();
ConfigureServices(builder);

var app = CreateApplication(builder);
app.Run();

public partial class Program {
  public static WebApplicationBuilder ConfigureServices(WebApplicationBuilder builder) {
    builder.Services.RegisterModules();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options => {
      options.SwaggerDoc("v2", new OpenApiInfo { Title = "ChocoManager", Version = "v2" });
      options.EnableAnnotations();
    });
    builder.Services.AddAuthorization();

    builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

    return builder;
  }

  public static WebApplication CreateApplication(WebApplicationBuilder builder) {
    var isRunningFromNUnit = 
      AppDomain.CurrentDomain.GetAssemblies().Any(
        a => a.FullName.ToLowerInvariant().StartsWith("nunit.framework"));
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI(options => { options.SwaggerEndpoint("v2/swagger.json", "ChocoManager v2"); });
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapEndpoints();

    if (isRunningFromNUnit)
    {
      return app;
    }

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();

    Log.Logger.Information("Checking if has pending migrations...");

    if (context.Database.GetPendingMigrations().Any())
    {
      Log.Logger.Information(
        "Found pending migrations: {GetPendingMigrations}, migrating...",
        context.Database.GetPendingMigrations());
      context.Database.Migrate();
    }

    return app;
  }
}