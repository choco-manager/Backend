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
using Backend.Exceptions;
using Backend.Modules.Orders.Contract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.Orders;

public class OrdersModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/api/orders", GetOrders).WithTags("Orders API");
    endpoints.MapGet("/api/orders/{id:guid}", GetOrdersDetails).WithTags("Orders API");
    
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
        .Select(o => mappers.Cut(o))
        .ToListAsync();
    }
    else
    {
      orders = await db.Orders
        .OrderByDescending(o => o.Date)
        .Skip(offset)
        .Take(count)
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
}