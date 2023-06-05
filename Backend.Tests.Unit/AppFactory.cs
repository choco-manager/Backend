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

using Backend.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

#endregion


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
      var descriptor =
        services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

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