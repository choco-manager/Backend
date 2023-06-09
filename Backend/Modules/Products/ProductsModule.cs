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
using Backend.Exceptions;
using Backend.Modules.PriceTypes.Contract;
using Backend.Modules.Products.Contract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;

#endregion


namespace Backend.Modules.Products;

public class ProductsModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/api/products", GetAllProducts).WithTags("Products API");
    endpoints.MapGet("/api/products/{id:guid}", GetProductDetails).WithTags("Products API");

    return endpoints;
  }

  private async Task<IResult> GetAllProducts([FromServices] ApplicationDbContext db, [FromServices] Mappers mappers) {
    using var op = Operation.Begin("Requesting products");
    var products = await db.Products.Select(product => mappers.Map(product)).ToListAsync();
    op.Complete();
    Log.Information("Fetched {Count} products", products.Count);
    return TypedResults.Ok(products);
  }

  private async Task<IResult> GetProductDetails(
    [FromRoute] Guid id,
    [FromServices] ApplicationDbContext db,
    [FromServices] Mappers mappers
  ) {
    using var productOp = Operation.Begin("Requesting product with Id = {Id}", id);
    var product = await db.Products
      .Where(p => p.Id == id)
      .Include(p => p.Category)
      .FirstOrDefaultAsync();
    productOp.Complete();

    if (product is null)
    {
      throw new EntityWasNotFoundException(nameof(Product), id);
    }

    using var pricesOp = Operation.Begin("Collecting prices change history of product with Id = {Id}", id);
    var retailPrices = await db.PriceChanges
      .OrderByDescending(pc => pc.ChangeTimestamp)
      .Where(pc => pc.Product == product && pc.PriceType.Id == PriceTypeEnum.Retail)
      .Take(10)
      .ToListAsync();

    var wholesalePrices = await db.PriceChanges
      .OrderByDescending(pc => pc.ChangeTimestamp)
      .Where(pc => pc.Product == product && pc.PriceType.Id == PriceTypeEnum.Wholesale)
      .Take(10)
      .ToListAsync();
    pricesOp.Complete();

    Log.Information("Fetched product '{Name}'", product.Name);
    return TypedResults.Ok(mappers.Enhance(product, retailPrices, wholesalePrices));
  }
}