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
using Backend.Data.Pagination;
using Backend.Exceptions;
using Backend.Modules.MovementItems.Contract;
using Backend.Modules.Products.Contract;
using Backend.Modules.Shipments.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.Shipments;

public class ShipmentsModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<AbstractValidator<UpdateShipmentRequestBody>, UpdateShipmentRequestBodyValidator>();
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    var module = endpoints
      .MapGroup("/api/shipments")
      .WithTags("Shipments API");

    module.MapGet("", GetShipments);
    module.MapGet("{id:guid}", GetShipmentDetails);
    module.MapPost("", CreateShipment);

    return endpoints;
  }

  [SwaggerOperation(Summary = "Gets all available shipments (with pagination and filtering by date)")]
  [SwaggerResponse(200, "Shipments was fetched successfully", typeof(Paged<Shipment>))]
  [SwaggerResponse(400, "Invalid data was passed", typeof(ProblemDetails))]
  [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
  private async Task<IResult> GetShipments(
    [FromServices] ApplicationDbContext db,
    [FromServices] Mappers mappers,
    [FromQuery] [SwaggerParameter("Number of page (ordered by date in descending order)")]
    int pageNumber = 0,
    [FromQuery] [SwaggerParameter("Amount of shipments to take from top (ordered by date in descending order)")]
    int count = 5,
    [FromQuery] [SwaggerParameter("Range of dates to take orders in from. Example: 2023-06-01:2023-06-10")]
    string? dateRange = null
  ) {
    if (!DateRange.TryParse(dateRange, out var dateRangeParsed))
    {
      throw new InvalidFormatOfDateRangeStringException(dateRange);
    }

    Paged<ShipmentDto> shipments;

    var op = Operation.Begin("Fetching shipments");

    if (dateRangeParsed is not null)
    {
      shipments = await Paged<ShipmentDto>.Split(
        db.Shipments
          .Where(s => s.Date >= dateRangeParsed.StartDate && s.Date <= dateRangeParsed.EndDate)
          .OrderByDescending(s => s.Date)
          .Include(s => s.Status)
          .Include(s => s.Items)
          .ThenInclude(mi => mi.Product)
          .Select(s => mappers.Cut(s)),
        pageNumber,
        count);
    }
    else
    {
      shipments = await Paged<ShipmentDto>.Split(
        db.Shipments
          .OrderByDescending(s => s.Date)
          .Include(s => s.Status)
          .Include(s => s.Items)
          .ThenInclude(mi => mi.Product)
          .Select(s => mappers.Cut(s)),
        pageNumber,
        count);
    }

    op.Complete();

    return TypedResults.Ok(shipments);
  }


  [SwaggerOperation(Summary = "Gets detail of shipment")]
  [SwaggerResponse(200, "Shipment was returned successfully", typeof(Shipment))]
  [SwaggerResponse(404, "Shipment was not found", typeof(ProblemDetails))]
  [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
  private async Task<IResult> GetShipmentDetails([FromServices] ApplicationDbContext db, [FromRoute] Guid id) {
    using var op = Operation.Begin("Requesting shipment with Id = {id}", id);

    var shipment = await db.Shipments
      .Include(s => s.Status)
      .Include(s => s.Items)
      .ThenInclude(mi => mi.Product)
      .FirstOrDefaultAsync(s => s.Id == id) ?? throw new EntityWasNotFoundException(nameof(Shipment), id);

    op.Complete();

    return TypedResults.Ok(shipment);
  }

  [SwaggerOperation(Summary = "Creates new order")]
  [SwaggerResponse(201, "Shipment was created successfully", typeof(Shipment))]
  [SwaggerResponse(404, "Some entity was not found", typeof(ProblemDetails))]
  [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
  private async Task<IResult> CreateShipment(
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<UpdateShipmentRequestBody> validator,
    [FromBody] UpdateShipmentRequestBody body
  ) {
    await validator.ValidateAndThrowAsync(body);

    using var op = Operation.Begin("Creating new shipment");

    var items = new List<MovementItem>();

    foreach (var item in body.Items)
    {
      items.Add(new MovementItem {
        Amount = item.Amount,
        Product = await db.Products.FindAsync(item.ProductId) ??
          throw new EntityWasNotFoundException(nameof(Product), item.ProductId),
      });
    }

    var shipment = new Shipment {
      Date = body.Date,
      Status = await db.MovementStatuses.FindAsync(body.StatusId) ??
        throw new EntityWasNotFoundException(nameof(MovementItem), body.StatusId),
      IsDeleted = false,
      Items = items,
    };

    await db.Shipments.AddAsync(shipment);
    await db.SaveChangesAsync();

    op.Complete();

    return TypedResults.Created("/api/shipments", shipment);
  }
}