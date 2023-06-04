using System.Net;
using System.Net.Http.Json;

using Backend.Modules.Cities.Contract;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;


namespace Backend.Tests.Unit.Modules.Cities;

[TestFixture]
public class CitiesModuleTests {
  private HttpClient _client = null!;
  private IServiceScope _scope = null!;


  [SetUp]
  public void Setup()
  {
    _scope = ModulesTestsSetup.Factory.Services.CreateScope();
    _client = ModulesTestsSetup.Factory.CreateClient();
  }

  [TearDown]
  public void TearDown() {
    _scope.Dispose();
    _client.Dispose();
  }


  [Test(Description = "Method to get list of cities should return status code 200 and list from 4 predefined cities")]
  public async Task GetCities__Returns200() {
    // Act
    var response = await _client.GetAsync("/api/cities");
    var cities = await response.Content.ReadFromJsonAsync<List<City>>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    cities.Should().NotBeNull();
    cities?.Count.Should().Be(4);
  }

  [Test(Description = "Method to create new city should return 400 when trying to create city with empty name")]
  public async Task PostCities__WhenCityNameIsEmpty__Returns400() {
    // Act
    var response = await _client.PostAsync(
      "/api/cities",
      JsonContent.Create(new CityRequestBody { Name = "" })
    );

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Test(Description = "Method to create new city should return 201 after creating city")]
  public async Task PostCities__Returns201() {
    // Act
    var response = await _client.PostAsync(
      "/api/cities",
      JsonContent.Create(new CityRequestBody { Name = "Москва" })
    );
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
  }
}