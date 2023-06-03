using Backend.Data;
using Backend.Modules.Cities.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;


namespace Backend.Modules.Cities;

public class CitiesModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<AbstractValidator<CityRequestBody>, CityRequestBodyValidator>();
    builder.AddSingleton<Mappers>();
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/api/cities", GetCitiesHandler);
    endpoints.MapPost("/api/cities", CreateCityHandler);

    return endpoints;
  }

  private static async Task<IResult> GetCitiesHandler([FromServices] ApplicationDbContext db) {
    using var op = Operation.Begin("Requesting cities");
    var cities = await db.Cities.ToListAsync();

    Log.Information("Fetched {Count} cities", cities.Count);
    return TypedResults.Ok(cities);
  }

  private static async Task<IResult> CreateCityHandler(
    [FromBody] CityRequestBody city,
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<CityRequestBody> validator,
    [FromServices] Mappers mapper
  ) {
    var result = await validator.ValidateAsync(city);

    if (!result.IsValid)
    {
      Log.Warning("Validation was not successful: {Errors}", result.Errors);
      return TypedResults.BadRequest(result.Errors);
    }

    using (var op = Operation.Begin("Saving new city"))
    {
      await db.Cities.AddAsync(mapper.Map(city));
      await db.SaveChangesAsync();
      op.Complete("City", city.Name);

    }
    Log.Information("Added new city '{Name}'", city.Name);
    return TypedResults.Created("/api/cities");
  }
}