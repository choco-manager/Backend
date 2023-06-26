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
using Backend.Data.Extensions;
using Backend.Exceptions;
using Backend.Modules.MovementItems.Contract;
using Backend.Modules.MovementStatuses.Contract;
using Backend.Modules.PriceChanges.Contract;
using Backend.Modules.PriceTypes.Contract;
using Backend.Modules.Products.Contract;
using Backend.Modules.Shipments.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.Products;

public class ProductsModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<AbstractValidator<UpdateProductRequestBody>, UpdateProductRequestBodyValidator>();

    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    var module = endpoints.MapGroup("/api/products").WithTags("Products API");
    module.MapGet("", GetAllProducts);
    module.MapGet("{id:guid}", GetProductDetails);
    module.MapPost("{id:guid}/delete", DeleteProduct);
    module.MapPost("{id:guid}/restore", RestoreProduct);
    module.MapPut("{id:guid}", UpdateProduct);
    module.MapPost("", CreateProduct);

    return endpoints;
  }

  [SwaggerOperation(Summary = "Gets all available products")]
  [SwaggerResponse(200, "Products was returned successfully", typeof(List<Product>))]
  private async Task<IResult> GetAllProducts([FromServices] ApplicationDbContext db, [FromServices] Mappers mappers) {
    using var op = Operation.Begin("Requesting products");
    var products = await db.Products
      .Include(p => p.Category)
      .Select(product => mappers.Cut(product, 0))
      .ToListAsync();

    foreach (var product in products)
    {
      product.Leftover = await db.MovementItems.GetLeftoverFor(product.Id);
    }

    op.Complete();
    Log.Information("Fetched {Count} products", products.Count);
    return TypedResults.Ok(products);
  }

  [SwaggerOperation(Summary = "Gets details of product")]
  [SwaggerResponse(200, "Product details was returned successfully", typeof(Product))]
  [SwaggerResponse(404, "Product was not found", typeof(ProblemDetails))]
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

    using var leftoverOp = Operation.Begin("Calculating leftover");

    var leftover = await db.MovementItems.GetLeftoverFor(product.Id);

    leftoverOp.Complete();

    Log.Information("Fetched product '{Name}'", product.Name);
    return TypedResults.Ok(mappers.Enhance(product, retailPrices, wholesalePrices, leftover));
  }

  [SwaggerOperation(Summary = "Marks product as deleted")]
  [SwaggerResponse(204, "Product was successfully marked as deleted")]
  [SwaggerResponse(404, "Product was not found", typeof(ProblemDetails))]
  private async Task<IResult> DeleteProduct(
    [FromRoute] Guid id,
    [FromServices] ApplicationDbContext db
  ) {
    using var op = Operation.Begin("Deleting product with Id = {Id}", id);
    var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id) ??
      throw new EntityWasNotFoundException(nameof(Product), id);

    product.IsDeleted = true;

    await db.SaveChangesAsync();
    op.Complete();

    return TypedResults.NoContent();
  }

  [SwaggerOperation(Summary = "Marks product as not deleted")]
  [SwaggerResponse(204, "Product was successfully marked as not deleted")]
  [SwaggerResponse(404, "Product was not found", typeof(ProblemDetails))]
  private async Task<IResult> RestoreProduct(
    [FromRoute] Guid id,
    [FromServices] ApplicationDbContext db
  ) {
    using var op = Operation.Begin("Restoring product with Id = {Id}", id);
    var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id) ??
      throw new EntityWasNotFoundException(nameof(Product), id);

    product.IsDeleted = false;

    await db.SaveChangesAsync();
    op.Complete();

    return TypedResults.Ok();
  }

  [SwaggerOperation(Summary = "Updates product")]
  [SwaggerResponse(200, "Product was successfully updated", typeof(ProductDto))]
  [SwaggerResponse(400, "Invalid body", typeof(ProblemDetails))]
  [SwaggerResponse(404, "Product was not found", typeof(ProblemDetails))]
  private async Task<IResult> UpdateProduct(
    [FromRoute] Guid id,
    [FromBody] UpdateProductRequestBody body,
    [FromServices] ApplicationDbContext db,
    [FromServices] Mappers mappers,
    [FromServices] AbstractValidator<UpdateProductRequestBody> validator
  ) {
    await validator.ValidateAndThrowAsync(body);

    var product = await db.Products
      .Include(p => p.Category)
      .Where(p => p.Id == id)
      .FirstOrDefaultAsync() ?? throw new EntityWasNotFoundException(nameof(Product), id);

    if (body.WholesalePrice != product.WholesalePrice)
    {
      product.WholesalePrice = body.WholesalePrice;
      await db.PriceChanges.AddAsync(new PriceChange {
        ChangeTimestamp = DateTime.UtcNow,
        Price = body.WholesalePrice,
        PriceType = await db.PriceTypes.FindAsync(PriceTypeEnum.Wholesale) ??
          throw new EntityWasNotFoundException(nameof(PriceType), PriceTypeEnum.Wholesale),
        Product = product,
      });
    }

    if (body.RetailPrice != product.RetailPrice)
    {
      product.RetailPrice = body.RetailPrice;
      await db.PriceChanges.AddAsync(new PriceChange {
        ChangeTimestamp = DateTime.UtcNow,
        Price = body.RetailPrice,
        PriceType = await db.PriceTypes.FindAsync(PriceTypeEnum.Retail) ??
          throw new EntityWasNotFoundException(nameof(PriceType), PriceTypeEnum.Retail),
        Product = product,
      });
    }

    product.Name = body.Name;
    product.IsByWeight = body.IsByWeight;
    product.VkMarketId = body.VkMarketId;

    await db.SaveChangesAsync();

    var leftover = await db.MovementItems.GetLeftoverFor(product.Id);

    return TypedResults.Ok(mappers.Cut(product, leftover));
  }

  [SwaggerOperation(Summary = "Creates product")]
  [SwaggerResponse(201, "Product was successfully created", typeof(ProductDto))]
  [SwaggerResponse(400, "Invalid body", typeof(ProblemDetails))]
  private async Task<IResult> CreateProduct(
    [FromBody] UpdateProductRequestBody body,
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<UpdateProductRequestBody> validator
  ) {
    await validator.ValidateAndThrowAsync(body);

    var product = new Product {
      Name = body.Name,
      Category = await db.ProductCategories.Where(pc => pc.Id == body.CategoryId).FirstAsync(),
      WholesalePrice = body.WholesalePrice,
      RetailPrice = body.RetailPrice,
    };

    await db.PriceChanges.AddRangeAsync(
      new PriceChange {
        Product = product,
        PriceType = await db.PriceTypes.FindAsync(PriceTypeEnum.Retail) ??
          throw new EntityWasNotFoundException(nameof(PriceType), PriceTypeEnum.Retail),
        Price = body.RetailPrice,
        ChangeTimestamp = DateTime.UtcNow,
      },
      new PriceChange {
        Product = product,
        PriceType = await db.PriceTypes.FindAsync(PriceTypeEnum.Wholesale) ??
          throw new EntityWasNotFoundException(nameof(PriceType), PriceTypeEnum.Wholesale),
        Price = body.WholesalePrice,
        ChangeTimestamp = DateTime.UtcNow,
      });
    await db.Products.AddAsync(product);

    await db.Shipments.AddAsync(new Shipment {
      Status = await db.MovementStatuses.FirstOrDefaultAsync(status => status.Name == "Выполнен") ??
        throw new EntityWasNotFoundException(nameof(MovementStatus), "Выполнен"),
      Items = new List<MovementItem> {
        new() {
          Product = product,
          Amount = body.Leftover,
        },
      },
    });


    await db.SaveChangesAsync();

    return TypedResults.Created("/api/products", product);
  }
}