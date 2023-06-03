using Backend.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;


namespace Backend.Tests.Unit;

public class AppFactory : WebApplicationFactory<Program> {
  private readonly string _dbConnectionString;
  public AppFactory(string host, int port, string password) {
    var sb = new NpgsqlConnectionStringBuilder {
      Host = host, Port = port, Database = "chocoTest", Username = "postgres", Password = password,
    };
    _dbConnectionString = sb.ConnectionString;
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder) {
    builder.ConfigureTestServices(services => {
      var descriptor = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

      if (descriptor is not null)
      {
        services.Remove(descriptor);
      }

      services.AddDbContextPool<ApplicationDbContext>(options => options.UseNpgsql(_dbConnectionString));

      var serviceProvider = services.BuildServiceProvider();
      using var scope = serviceProvider.CreateScope();
      var scopedServices = scope.ServiceProvider;
      var context = scopedServices.GetRequiredService<ApplicationDbContext>();
      context.Database.EnsureDeleted();
      context.Database.EnsureCreated();
    });
  }
}