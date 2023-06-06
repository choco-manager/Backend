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
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

using Backend.Modules.Addresses.Contract;
using Backend.Modules.Cities.Contract;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

#endregion


namespace Backend.Tests.Unit.Modules.Addresses;

[TestFixture]
public class AddressesModuleTests {
  private HttpClient _client = null!;
  private IServiceScope _scope = null!;

  [SetUp]
  public void Setup() {
    _scope = ModulesTestsSetup.Factory.Services.CreateScope();
    _client = ModulesTestsSetup.Factory.CreateClient();
  }

  [TearDown]
  public void TearDown() {
    _scope.Dispose();
    _client.Dispose();
  }

  [Order(1)]
  [Test(Description =
    "Method to generate random addresses should return status code 201 and list of generated addresses")]
  public async Task PutAddresses__Returns201() {
    // Act
    var response = await _client.PutAsync("/api/addresses", null);
    var addresses = await response.Content.ReadFromJsonAsync<List<Address>>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    addresses.Should().NotBeNull();
    addresses?.Count.Should().BeInRange(10, 20);
  }

  [Order(2)]
  [Test(Description = "Method to get list of addresses should return status code 200 and list from 5 addresses")]
  public async Task GetAddresses__Returns200() {
    // Act
    var response = await _client.GetAsync("/api/addresses");
    var addresses = await response.Content.ReadFromJsonAsync<List<Address>>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    addresses.Should().NotBeNull();
    addresses?.Count.Should().Be(5);
  }

  [Test(Description = "Method to create new order should create new order and return it")]
  public async Task CreateAddress__Returns201() {
    var response = await _client.GetAsync("/api/cities");
    var citites = await response.Content.ReadFromJsonAsync<List<City>>();
    var city = citites[0];

    var street = "Мичурина улица";
    var building = "40б";
    var rb = new CreateAddressRequestBody {
      City = city.Id,
      Street = street,
      Building = building,
    };

    // Act
    var serialize = JsonSerializer.Serialize(rb);
    var postResponse = await _client.PostAsync("/api/addresses",
      new StringContent(serialize, Encoding.UTF8, MediaTypeNames.Application.Json));
    var postResult = await postResponse.Content.ReadFromJsonAsync<Address>();

    // Assert
    postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    postResult.City.Id.Should().Be(city.Id);
    postResult.Street.Should().Be(street);
    postResult.Building.Should().Be(building);
  }

  [Test(Description = "Method to create new order should not create new order if it is already exists")]
  public async Task CreateAddress__Returns200() {
    // Arrrange
    var response = await _client.GetAsync("/api/addresses");
    var addresses = await response.Content.ReadFromJsonAsync<List<Address>>();
    var address = addresses[0];

    var rb = new CreateAddressRequestBody {
      City = address.City.Id,
      Street = address.Street,
      Building = address.Building,
    };

    // Act
    var serialize = JsonSerializer.Serialize(rb);
    var postResponse = await _client.PostAsync("/api/addresses",
      new StringContent(serialize, Encoding.UTF8, MediaTypeNames.Application.Json));
    var postResult = await postResponse.Content.ReadFromJsonAsync<Address>();


    // Assert
    postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    postResult.Id.Should().Be(address.Id);
  }
}