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
using Backend.Data.DateRange;
using Backend.Data.Extensions;
using Backend.Exceptions;
using Backend.Modules.Addresses.Contract;
using Backend.Modules.Clients.Contract;
using Backend.Modules.MovementItems.Contract;
using Backend.Modules.MovementItems.Utils;
using Backend.Modules.MovementStatuses.Contract;
using Backend.Modules.MovementStatuses.Utils;
using Backend.Modules.Orders.Contract;
using Backend.Modules.Products.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.Orders;

public class OrdersModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<AbstractValidator<UpdateOrderRequestBody>, UpdateOrderRequestBodyValidator>();
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    var module = endpoints.MapGroup("/api/orders").WithTags("Orders API");

    module.MapGet("", GetOrders);
    module.MapGet("{id:guid}", GetOrdersDetails);
    module.MapPost("", CreateOrder);
    module.MapPost("{id:guid}/delete", MarkOrderAsDeleted);
    module.MapPost("{id:guid}/restore", MarkOrderAsNotDeleted);
    module.MapPut("{id:guid}", UpdateOrder);

    return endpoints;
  }

  [SwaggerOperation(Summary = "Gets all available orders (with pagination and filtering by date)")]
  [SwaggerResponse(200, "Orders was fetched successfully", typeof(List<Order>))]
  [SwaggerResponse(400, "Invalid data was passed", typeof(ProblemDetails))]
  private async Task<IResult> GetOrders(
    [FromServices] ApplicationDbContext db,
    [FromServices] Mappers mappers,
    [FromQuery] [SwaggerParameter("Amount of orders to skip from top (ordered by date in descending order)")]
    int offset = 0,
    [FromQuery] [SwaggerParameter("Amount of orders to take from top (ordered by date in descending order)")]
    int count = 5,
    [FromQuery] [SwaggerParameter("Range of dates to take orders in from. Example: 2023-06-01:2023-06-10")]
    string? dateRange = null
  ) {
    if (!DateRange.TryParse(dateRange, out var dateRangeParsed))
    {
      return TypedResults.BadRequest(dateRange);
    }

    List<OrderDto> orders;

    if (dateRangeParsed is not null)
    {
      orders = await db.Orders
        .Where(o => o.Date >= dateRangeParsed.StartDate && o.Date <= dateRangeParsed.EndDate)
        .OrderByDescending(o => o.Date)
        .Skip(offset)
        .Take(count)
        .Include(o => o.Client)
        .Include(o => o.Status)
        .Include(o => o.Items)
        .ThenInclude(i => i.Product)
        .Include(o => o.SelectedAddress)
        .ThenInclude(a => a.City)
        .Select(o => mappers.Cut(o))
        .ToListAsync();
    }
    else
    {
      orders = await db.Orders
        .OrderByDescending(o => o.Date)
        .Skip(offset)
        .Take(count)
        .Include(o => o.Client)
        .Include(o => o.Status)
        .Include(o => o.Items)
        .ThenInclude(i => i.Product)
        .Include(o => o.SelectedAddress)
        .ThenInclude(a => a.City)
        .Select(o => mappers.Cut(o))
        .ToListAsync();
    }

    return TypedResults.Ok(orders);
  }

  [SwaggerOperation(Summary = "Gets detail of order")]
  [SwaggerResponse(200, "Order was returned successfully", typeof(Order))]
  [SwaggerResponse(404, "Order was not found", typeof(ProblemDetails))]
  private async Task<IResult> GetOrdersDetails([FromServices] ApplicationDbContext db, [FromRoute] Guid id) {
    using var op = Operation.Begin("Requesting order with Id = {Id}", id);
    var order = await db.Orders
        .Include(o => o.Client)
        .Include(o => o.Items)
        .ThenInclude(i => i.Product)
        .Include(o => o.SelectedAddress)
        .ThenInclude(a => a.City)
        .FirstOrDefaultAsync(o => o.Id == id) ??
      throw new EntityWasNotFoundException(nameof(Order), id);

    op.Complete();

    return TypedResults.Ok(order);
  }

  [SwaggerOperation(Summary = "Creates new order")]
  [SwaggerResponse(201, "Order was created successfully", typeof(Order))]
  [SwaggerResponse(404, "Some entity was not found", typeof(ProblemDetails))]
  private static async Task<IResult> CreateOrder(
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<UpdateOrderRequestBody> validator,
    [FromBody] UpdateOrderRequestBody body
  ) {
    await validator.ValidateAndThrowAsync(body);

    using var op = Operation.Begin("Creating new order");
    var items = new List<MovementItem>();

    foreach (var item in body.Items)
    {
      items.Add(new MovementItem {
        Amount = item.Amount,
        Product = await db.Products.FindAsync(item.ProductId) ??
          throw new EntityWasNotFoundException(nameof(Product), item.ProductId),
      });
    }

    var order = new Order {
      Date = body.Date,
      Status = await db.MovementStatuses.FindAsync(body.MovementStatusId) ??
        throw new EntityWasNotFoundException(nameof(MovementStatus), body.MovementStatusId),
      Client = await db.Clients.FindAsync(body.ClientId) ??
        throw new EntityWasNotFoundException(nameof(Client), body.ClientId),
      SelectedAddress = await db.Addresses.FindAsync(body.SelectedAddressId) ??
        throw new EntityWasNotFoundException(nameof(Address), body.SelectedAddressId),
      IsDeleted = false,
      Items = items,
    };

    await db.Orders.AddAsync(order);
    await db.SaveChangesAsync();

    op.Complete();

    return TypedResults.Created("/api/orders", order);
  }

  [SwaggerOperation(Summary = "Marks order as deleted")]
  [SwaggerResponse(204, "Order was marked as deleted successfully")]
  [SwaggerResponse(404, "Order was not found", typeof(ProblemDetails))]
  private async Task<IResult> MarkOrderAsDeleted([FromServices] ApplicationDbContext db, [FromRoute] Guid id) {
    var order = await db.Orders.FindAsync(id) ??
      throw new EntityWasNotFoundException(nameof(Order), id);

    order.IsDeleted = true;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
  }

  [SwaggerOperation(Summary = "Marks order as not deleted")]
  [SwaggerResponse(200, "Order was returned from trash successfully")]
  [SwaggerResponse(404, "Order was not found", typeof(ProblemDetails))]
  private async Task<IResult> MarkOrderAsNotDeleted([FromServices] ApplicationDbContext db, [FromRoute] Guid id) {
    var order = await db.Orders.FindAsync(id) ??
      throw new EntityWasNotFoundException(nameof(Order), id);

    order.IsDeleted = false;

    await db.SaveChangesAsync();

    return TypedResults.Ok();
  }

  [SwaggerOperation(Summary = "Updates order")]
  [SwaggerResponse(200, "Order was updated successfully", typeof(Order))]
  [SwaggerResponse(404, "Some entity was not found", typeof(ProblemDetails))]
  private async Task<IResult> UpdateOrder(
    [FromServices] ApplicationDbContext db,
    [FromRoute] Guid id,
    [FromBody] UpdateOrderRequestBody body
  ) {
    // TODO: Валидация

    var order = await db.Orders.FindAsync(id) ??
      throw new EntityWasNotFoundException(nameof(Order), id);

    order.Date = body.Date;
    var newStatus = await db.MovementStatuses.FindAsync(body.MovementStatusId) ??
      throw new EntityWasNotFoundException(nameof(MovementStatus), body.MovementStatusId);

    order.Status = order.Status.IsChangePossible(newStatus)
      ? newStatus
      : throw new CouldNotChangeStatusException(order.Status, newStatus, nameof(Order), id);

    order.Client = await db.Clients.FindAsync(body.ClientId) ??
      throw new EntityWasNotFoundException(nameof(Client), body.ClientId);

    order.SelectedAddress = await db.Addresses.FindAsync(body.SelectedAddressId) ??
      throw new EntityWasNotFoundException(nameof(Address), body.SelectedAddressId);

    var items = new List<MovementItem>();

    foreach (var item in body.Items)
    {
      items.Add(new MovementItem {
        Amount = item.Amount,
        Product = await db.Products.FindAsync(item.ProductId) ??
          throw new EntityWasNotFoundException(nameof(Product), item.ProductId),
      });
    }

    var diff = order.Items.GetDifferencesFrom(items);

    foreach (var item in diff)
    {
      var product = item.Product;
      var leftover = await db.MovementItems.GetLeftoverFor(product.Id);

      if (leftover - item.Amount < 0)
      {
        throw new InsufficientProductLeftoverException(product, leftover, item.Amount);
      }
    }
    
    order.Items = order.Items.ApplyDifferences(diff);

    await db.SaveChangesAsync();

    return TypedResults.Ok(order);
  }
}