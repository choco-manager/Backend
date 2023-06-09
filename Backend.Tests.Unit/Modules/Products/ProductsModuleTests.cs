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

using Backend.Modules.Products.Contract;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

#endregion


namespace Backend.Tests.Unit.Modules.Products;

[TestFixture]
public class ProductsModuleTests {
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

  [Test(Description = "Create product should return 201")]
  [Order(1)]
  public async Task CreateProduct__Returns201() {
    // Act
    var response = await _client.PostAsync("/api/products", JsonContent.Create(new UpdateProductRequestBody {
      Name = "Молочный",
      IsByWeight = false,
      WholesalePrice = 300,
      RetailPrice = 600,
      CategoryId = new Guid("57902a45-c181-4450-ad49-cdaed2faef01"),
      VkMarketId = 0,
    }));

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
  }

  [Test(Description =
    "Method to get list of products should return status code 200 and list of products")]
  public async Task GetProducts__Returns200() {
    // Act
    var response = await _client.GetAsync("/api/products");
    var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    products.Should().NotBeNull();
    products?.Count.Should().Be(1);
  }

  [Test(Description = "Method to delete product should mark product as deleted")]
  public async Task DeleteProduct__Returns204() {
    var getResponse = await _client.GetAsync("/api/products");
    var productsList = await getResponse.Content.ReadFromJsonAsync<List<ProductDto>>();


    var deleteResponse = await _client.PostAsync($"/api/products/{productsList[0].Id}/delete", null);
    deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

    getResponse = await _client.GetAsync($"/api/products/{productsList[0].Id}");
    var product = await getResponse.Content.ReadFromJsonAsync<ProductDetails>();

    getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    product.IsDeleted.Should().BeTrue();
  }


  [Test(Description = "Method to delete product should mark product as deleted")]
  public async Task RestoreProduct__Returns200() {
    var getResponse = await _client.GetAsync("/api/products");
    var productsList = await getResponse.Content.ReadFromJsonAsync<List<ProductDto>>();


    var deleteResponse = await _client.PostAsync($"/api/trash/products/{productsList[0].Id}", null);
    deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

    getResponse = await _client.GetAsync($"/api/products/{productsList[0].Id}");
    var product = await getResponse.Content.ReadFromJsonAsync<ProductDetails>();

    getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    product.IsDeleted.Should().BeFalse();
  }
}