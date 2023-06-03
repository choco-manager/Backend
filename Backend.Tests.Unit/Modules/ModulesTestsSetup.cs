using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using NUnit.Framework;


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

    Factory = new(_pgContainer.Hostname, _pgContainer.GetMappedPublicPort(5432), postgresPwd);
  }
  
  [OneTimeTearDown]
  public async Task DisposeContainer() {
    await _pgContainer.DisposeAsync();
  }
}