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
using Backend.Modules.Inventories.Contract;
using Backend.Modules.MovementItems.Contract;
using Backend.Modules.Products.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.Inventories;

public class InventoriesModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<AbstractValidator<TakeInventoryRequestBody>, TakeInventoryRequestBodyValidator>();
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    var module = endpoints.MapGroup("/api/inventory").WithTags("Inventory API");

    module.MapPost("", TakeInventory);

    return endpoints;
  }

  [SwaggerOperation("Takes inventory of products", "Replaces leftovers of products with provided ones")]
  [SwaggerResponse(201, "Inventory was taken successfully")]
  [SwaggerResponse(404, "One or more products was not found", typeof(ProblemDetails))]
  [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
  private async Task<IResult> TakeInventory(
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<TakeInventoryRequestBody> validator,
    [FromBody] [SwaggerRequestBody("List of products to apply new leftovers for")]
    TakeInventoryRequestBody body
  ) {
    await validator.ValidateAndThrowAsync(body);

    var items = new List<MovementItem>();

    foreach (var item in body.Items)
    {
      var product = await db.Products.FindAsync(item.ProductId) ??
        throw new EntityWasNotFoundException(nameof(Product), item.ProductId);

      items.Add(new MovementItem {
        Product = product,
        Amount = item.Amount - await db.MovementItems.GetLeftoverFor(item.ProductId),
      });
    }

    await db.Inventories.AddAsync(new Inventory {
      Date = DateOnly.FromDateTime(DateTime.Today),
      Items = items,
    });

    await db.SaveChangesAsync();

    return TypedResults.Created("/api/inventory");
  }
}