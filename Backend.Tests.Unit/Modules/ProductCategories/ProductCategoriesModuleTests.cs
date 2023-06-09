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

using Backend.Modules.ProductCategories.Contract;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

#endregion


namespace Backend.Tests.Unit.Modules.ProductCategories;

[TestFixture]
public class ProductCategoriesModuleTests {
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
    "Method to get list of product categories should return status code 200 and list from 7 predefined cities")]
  public async Task GetProductCategories__Returns200() {
    // Act
    var response = await _client.GetAsync("/api/productCategories");
    var movementStatuses = await response.Content.ReadFromJsonAsync<List<ProductCategory>>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    movementStatuses.Should().NotBeNull();
    movementStatuses?.Count.Should().Be(7);
  }

  [Test(Description =
    "Method to create new product category should return 400 when trying to create product category with empty name")]
  public async Task PostProductCategory__WhenProductCategoryNameIsEmpty__Returns400() {
    // Act
    var response = await _client.PostAsync(
      "/api/productCategories",
      JsonContent.Create(new CreateProductCategoryRequestBody() { Name = "" })
    );

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Test(Description = "Method to create new product category should return 201 after creating product category")]
  public async Task PostProductCategory__Returns201() {
    // Act
    var response = await _client.PostAsync(
      "/api/productCategories",
      JsonContent.Create(new CreateProductCategoryRequestBody() { Name = "Пасты" })
    );

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
  }
}