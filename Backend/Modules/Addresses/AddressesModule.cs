﻿// ------------------------------------------------------------------------
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
using Backend.Data.FakeDataGeneration;
using Backend.Data.Pagination;
using Backend.Exceptions;
using Backend.Modules.Addresses.Contract;
using Backend.Modules.Cities.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.Addresses;

[SwaggerTag("Operations related to addresses")]
public class AddressesModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<AbstractValidator<CreateAddressRequestBody>, CreateAddressRequestBodyValidator>();

    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    var module = endpoints
      .MapGroup("/api/addresses")
      .WithTags("Addresses API");

    module.MapGet("", GetAddresses);
    module.MapPost("", CreateAddress);
    module.MapPut("fake", GenerateAddresses);

    return endpoints;
  }

  [SwaggerOperation(Summary = "Gets all available addresses (with pagination)")]
  [SwaggerResponse(200, "Addresses was returned successfully", typeof(Paged<Address>))]
  [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
  private async Task<IResult> GetAddresses(
    [FromServices] ApplicationDbContext db,
    [FromQuery] [SwaggerParameter("Number of page to fetch")]
    int pageNumber = 0,
    [FromQuery] [SwaggerParameter("Amount of elements on one page")]
    int count = 5
  ) {
    using var op = Operation.Begin("Requesting addresses");
    var addresses = await Paged<Address>.Split(db.Addresses
      .Include(a => a.City), pageNumber, count);
    op.Complete();
    Log.Information("Fetched {Count} addresses", addresses.TotalCount);
    return TypedResults.Ok(addresses);
  }

  [SwaggerOperation(Summary = "Creates new address or gets existing one")]
  [SwaggerResponse(200, "Found existing address with passed data, no address was created", typeof(Address))]
  [SwaggerResponse(201, "Address was created successfully", typeof(Address))]
  [SwaggerResponse(400, "Invalid data was passed", typeof(ProblemDetails))]
  [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
  private async Task<IResult> CreateAddress(
    [FromBody] [SwaggerRequestBody("Parameters to create or get address")]
    CreateAddressRequestBody body,
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<CreateAddressRequestBody> validator
  ) {
    await validator.ValidateAndThrowAsync(body);

    var cityOp = Operation.Begin("Searching for city by Id");

    var city = await db.Cities.FirstOrDefaultAsync(city => city.Id == body.City) ??
      throw new EntityWasNotFoundException(nameof(City), body.City);

    cityOp.Complete();

    var addressOp = Operation.Begin("Searching for address by its parameters");

    var address = await db.Addresses.FirstOrDefaultAsync(address =>
      address.City.Name == city.Name && address.Street == body.Street && address.Building == body.Building);

    addressOp.Complete();

    if (address is not null)
    {
      Log.Information("Requested address was found, returning it, instead of creating new one");
      return TypedResults.Ok(address);
    }


    var createAddressOp = Operation.Begin("Creating new address");

    var createdAddress = new Address {
      City = city,
      Street = body.Street,
      Building = body.Building,
    };

    await db.AddAsync(createdAddress);
    await db.SaveChangesAsync();

    createAddressOp.Complete();

    return TypedResults.Created("/api/addresses", createdAddress);
  }

  [SwaggerOperation(Summary = "Generates new fake addresses")]
  [SwaggerResponse(201, "Addresses was created successfully", typeof(List<Address>))]
  [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
  private async Task<IResult> GenerateAddresses([FromServices] ApplicationDbContext db) {
    var generator = new GenerateAddresses();
    var op = Operation.Begin("Generating fake addreses");
    var addresses = await generator.Generate(db);
    op.Complete();
    return TypedResults.Created("/api/addresses", addresses);
  }
}