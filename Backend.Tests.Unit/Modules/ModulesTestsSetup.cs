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

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using NUnit.Framework;

#endregion


namespace Backend.Tests.Unit.Modules;

[SetUpFixture]
public class ModulesTestsSetup {
  public static AppFactory Factory;
  private IContainer _pgContainer;

  [OneTimeSetUp]
  public async Task CreateContainer() {
    const string postgresPwd = "pgpwd";

    _pgContainer = new ContainerBuilder()
      .WithName(Guid.NewGuid().ToString("N"))
      .WithImage("postgres:15")
      .WithHostname("chocoTest")
      .WithExposedPort(5432)
      .WithPortBinding(5432, true)
      .WithEnvironment("POSTGRES_PASSWORD", postgresPwd)
      .WithEnvironment("PGDATA", "/pgdata")
      .WithTmpfsMount("/pgdata")
      .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("psql -U postgres -c \"select 1\""))
      .Build();
    await _pgContainer.StartAsync();

    Factory = new AppFactory(_pgContainer.Hostname, _pgContainer.GetMappedPublicPort(5432), postgresPwd);
  }

  [OneTimeTearDown]
  public async Task DisposeContainer() {
    await _pgContainer.DisposeAsync();
  }
}