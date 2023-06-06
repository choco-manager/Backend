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

using Backend.Modules.MovementStatuses.Contract;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

#endregion


namespace Backend.Tests.Unit.Modules.MovementStatuses;

[TestFixture]
public class MovementStatusesModuleTests {
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


  [Test(Description =
    "Method to get list of movement statuses should return status code 200 and list from 5 predefined cities")]
  public async Task GetMovementStatuses__Returns200() {
    // Act
    var response = await _client.GetAsync("/api/movementStatuses");
    var movementStatuses = await response.Content.ReadFromJsonAsync<List<MovementStatus>>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    movementStatuses.Should().NotBeNull();
    movementStatuses?.Count.Should().Be(5);
  }

  [Test(Description =
    "Method to create new movement status should return 400 when trying to create movement status with empty name")]
  public async Task PostMovementStatus__WhenMovementStatusNameIsEmpty__Returns400() {
    // Act
    var response = await _client.PostAsync(
      "/api/movementStatuses",
      JsonContent.Create(new CreateMovementStatusRequestBody { Name = "" })
    );

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Test(Description = "Method to create new movement status should return 201 after creating city")]
  public async Task PostMovementStatus__Returns201() {
    // Act
    var response = await _client.PostAsync(
      "/api/movementStatuses",
      JsonContent.Create(new CreateMovementStatusRequestBody { Name = "Готово" })
    );

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
  }
}