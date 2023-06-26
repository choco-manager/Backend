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

using Backend.Data.Pagination;
using Backend.Modules.Clients.Contract;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

#endregion


namespace Backend.Tests.Unit.Modules.Clients;

[TestFixture]
public class ClientsModuleTests {
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
    "Method to generate random clients should return status code 201 and list of generated clients")]
  public async Task PutClients__Returns201() {
    // Act
    var response = await _client.PutAsync("/api/clients/fake", null);
    var clients = await response.Content.ReadFromJsonAsync<List<Client>>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    clients.Should().NotBeNull();
    clients?.Count.Should().BeInRange(10, 20);
  }

  [Order(2)]
  [Test(Description = "Method to get list of clients should return status code 200 and list from 5 clients")]
  public async Task GetClients__Returns200() {
    // Act
    var response = await _client.GetAsync("/api/clients");
    var addresses = await response.Content.ReadFromJsonAsync<Paged<ClientDto>>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    addresses.Should().NotBeNull();
    addresses?.PageSize.Should().Be(5);
  }

  [Test(Description = "Method to create new client should return status code 201 and created client")]
  public async Task CreateClient__Returns201() {
    // Act
    var response = await _client.PostAsync("/api/clients",
      JsonContent.Create(new UpdateClientRequestBody {
        FirstName = "John",
        LastName = "Doe",
        ChatLink = "https://vk.com/gim1234567_1",
        PhoneNumber = "(678) 673-34-23",
        Addresses = new List<Guid>(),
      }));

    var client = await response.Content.ReadFromJsonAsync<Client>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    client.Should().NotBeNull();
  }
}