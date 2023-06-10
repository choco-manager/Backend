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

using Backend.Data;
using Backend.Data.FakeDataGeneration;
using Backend.Modules.Addresses.Contract;

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
    endpoints.MapGet("/api/addresses", GetAddresses).WithTags("Addresses API");
    endpoints.MapPost("/api/addresses", CreateAddress).WithTags("Addresses API");
    endpoints.MapPut("/api/addresses/fake", GenerateAddresses).WithTags("Addresses API");

    return endpoints;
  }

  [SwaggerOperation(Summary = "Gets all available addresses (with pagination)")]
  [SwaggerResponse(200, "Addresses was returned successfully", typeof(List<Address>))]
  private async Task<IResult> GetAddresses(
    [FromServices] ApplicationDbContext db,
    [FromQuery] int offset = 0,
    [FromQuery] int count = 5
  ) {
    using var op = Operation.Begin("Requesting addresses");
    var addresses = await db.Addresses
      .Skip(offset)
      .Take(count)
      .Include(a => a.City)
      .ToListAsync();
    op.Complete();
    Log.Information("Fetched {Count} addresses", addresses.Count);
    return TypedResults.Ok(addresses);
  }

  [SwaggerOperation(Summary = "Creates new address or gets existing one")]
  [SwaggerResponse(200, "Found existing address with passed data, no address was created", typeof(Address))]
  [SwaggerResponse(201, "Address was created successfully", typeof(Address))]
  [SwaggerResponse(400, "Invalid data was passed", typeof(ProblemDetails))]
  private async Task<IResult> CreateAddress(
    [FromBody] CreateAddressRequestBody body,
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<CreateAddressRequestBody> validator
  ) {
    await validator.ValidateAndThrowAsync(body);

    var city = await db.Cities.FirstOrDefaultAsync(city => city.Id == body.City);
    if (city is null)
    {
      return TypedResults.BadRequest(new ProblemDetails {
        Status = (int)HttpStatusCode.BadRequest,
        Title = $"City '{body.City}' was not found",
      });
    }

    var address = await db.Addresses.FirstOrDefaultAsync(address =>
      address.City == city && address.Street == body.Street && address.Building == body.Building);

    if (address is not null)
    {
      return TypedResults.Ok(address);
    }

    var createdAddress = new Address {
      City = city,
      Street = body.Street,
      Building = body.Building,
    };

    await db.AddAsync(createdAddress);
    await db.SaveChangesAsync();

    return TypedResults.Created("/api/addresses", createdAddress);
  }

  [SwaggerOperation(Summary = "Generates new fake addresses")]
  [SwaggerResponse(201, "Addresses was created successfully", typeof(List<Address>))]
  private async Task<IResult> GenerateAddresses([FromServices] ApplicationDbContext db) {
    var generator = new GenerateAddresses();
    var addresses = await generator.Generate(db);
    return TypedResults.Created("/api/addresses", addresses);
  }
}