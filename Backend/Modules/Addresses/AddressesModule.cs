using System.Net;

using Backend.Data;
using Backend.Data.FakeDataGeneration;
using Backend.Modules.Addresses.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;


namespace Backend.Modules.Addresses;

public class AddressesModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<AbstractValidator<CreateAddressRequestBody>, CreateAddressRequestBodyValidator>();

    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/api/addresses", GetAddresses);
    endpoints.MapPost("/api/addresses", CreateAddress);
    endpoints.MapPut("/api/addresses", GenerateAddresses);

    return endpoints;
  }

  private async Task<IResult> GetAddresses(
    [FromServices] ApplicationDbContext db,
    [FromQuery] int offset = 0,
    [FromQuery] int count = 5
  ) {
    using var op = Operation.Begin("Requesting cities");
    var addresses = await db.Addresses
      .Skip(offset)
      .Take(count)
      .Include(a => a.City)
      .ToListAsync();
    op.Complete();
    Log.Information("Fetched {Count} addresses", addresses.Count);
    return TypedResults.Ok(addresses);
  }

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

  private async Task<IResult> GenerateAddresses([FromServices] ApplicationDbContext db) {
    var generator = new GenerateAddresses();
    var addresses = await generator.Generate(db);
    return TypedResults.Created("/api/addresses", addresses);
  }
}