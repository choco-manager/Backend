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
using Backend.Modules.ProductCategories.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.ProductCategories;

public class ProductCategoriesModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder
      .AddSingleton<AbstractValidator<CreateProductCategoryRequestBody>, CreateProductCategoryRequestBodyValidator>();
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/api/productCategories", GetAllProductCategoriesHandler).WithTags("Product Categories API");
    endpoints.MapPost("/api/productCategories", CreateProductCategoryHandler).WithTags("Product Categories API");

    return endpoints;
  }

  [SwaggerOperation(Summary = "Gets all available product categories")]
  [SwaggerResponse(200, "Cities was returned successfully", typeof(List<ProductCategory>))]
  private async Task<IResult> GetAllProductCategoriesHandler([FromServices] ApplicationDbContext db) {
    using var op = Operation.Begin("Requesting cities");
    var productCategories = await db.ProductCategories.ToListAsync();
    op.Complete();
    Log.Information("Fetched {Count} product categories", productCategories.Count);
    return TypedResults.Ok(productCategories);
  }

  [SwaggerOperation(Summary = "Creates new product category")]
  [SwaggerResponse(201, "Product category was created successfully", typeof(ProductCategory))]
  [SwaggerResponse(400, "Invalid data was passed", typeof(ProblemDetails))]
  private async Task<IResult> CreateProductCategoryHandler(
    [FromBody] CreateProductCategoryRequestBody body,
    [FromServices] AbstractValidator<CreateProductCategoryRequestBody> validator,
    [FromServices] Mappers mapper,
    [FromServices] ApplicationDbContext db
  ) {
    await validator.ValidateAndThrowAsync(body);

    using var op = Operation.Begin("Saving new product category");
    var entity = mapper.Enhance(body);
    await db.ProductCategories.AddAsync(entity);
    await db.SaveChangesAsync();
    op.Complete("Product category", body.Name);

    Log.Information("Added new product category '{Name}'", body.Name);
    return TypedResults.Created("/api/productCategories", entity);
  }
}