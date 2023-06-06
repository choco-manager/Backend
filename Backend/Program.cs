// ------------------------------------------------------------------------
// Copyright (C) 2023 dadyarri
// This file is part of ChocoManager <https://github.com/choco-manager/Backend>.
// 
// ChocoManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// ChocoManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with ChocoManager.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------
// 

#region

using System.Net;

using Backend.Data;
using Backend.Middlewares;
using Backend.Modules;

using LettuceEncrypt;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Serilog;
using Serilog.Events;

#endregion


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
  .MinimumLevel.Override("Default", LogEventLevel.Debug)
  .CreateLogger();

builder.Host.UseSerilog();
ConfigureServices(builder);

if (!builder.Environment.IsDevelopment())
{
  builder.Services
    .AddLettuceEncrypt()
    .PersistDataToDirectory(
      new DirectoryInfo(builder.Configuration["Certificates:Path"]!),
      builder.Configuration["Certificates:Password"]
    );


  builder.WebHost.UseKestrel(k => {
    var appServices = k.ApplicationServices;
    k.Listen(
      IPAddress.Any, 443,
      o => o.UseHttps(h => { h.UseLettuceEncrypt(appServices); }));
  });
}

var app = CreateApplication(builder);
app.Run();

public partial class Program {
  private static void ConfigureServices(WebApplicationBuilder builder) {
    builder.Services.RegisterModules();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options => {
      options.SwaggerDoc("v2",
        new OpenApiInfo {
          Title = "ChocoManager", Version = "v2", Description = "Main API of ChocoManager project",
          Contact = new OpenApiContact
            { Name = "Daniil", Email = "me@dadyarri.ru", Url = new Uri("https://dadyarri.ru", UriKind.Absolute) },
          License = new OpenApiLicense
            { Name = "GNU GPLv3", Url = new Uri("https://choosealicense.com/licenses/gpl-3.0/", UriKind.Absolute) },
        });
      options.EnableAnnotations();
    });
    builder.Services.AddAuthorization();

    builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
    builder.Services.AddSingleton<Mappers>();

    builder.Services.AddTransient<CheckPendingDatabaseChangesMiddleware>();
    builder.Services.AddTransient<GlobalErrorHandlingMiddleware>();
  }

  private static WebApplication CreateApplication(WebApplicationBuilder builder) {
    var isRunningFromNUnit =
      Array.Exists(AppDomain.CurrentDomain.GetAssemblies(),
        a => a.FullName?.ToLowerInvariant().StartsWith("nunit.framework") ?? false);
    var app = builder.Build();

    if (!app.Environment.IsProduction())
    {
      app.UseSwagger();
      app.UseSwaggerUI(options => { options.SwaggerEndpoint("v2/swagger.json", "ChocoManager v2"); });
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapEndpoints();

    app.UseMiddleware<CheckPendingDatabaseChangesMiddleware>();
    app.UseMiddleware<GlobalErrorHandlingMiddleware>();

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