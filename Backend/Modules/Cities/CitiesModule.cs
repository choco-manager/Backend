using Backend.Data;
using Backend.Modules.Cities.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;


namespace Backend.Modules.Cities;

public class CitiesModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<AbstractValidator<CreateCityRequestBody>, CityRequestBodyValidator>();
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/api/cities", GetCitiesHandler);
    endpoints.MapPost("/api/cities", CreateCityHandler);

    return endpoints;
  }

  [SwaggerOperation(Summary = "Gets all available cities")]
  [SwaggerResponse(200, "Cities was returned successfully", typeof(List<City>))]
  private static async Task<IResult> GetCitiesHandler([FromServices] ApplicationDbContext db) {
    using var op = Operation.Begin("Requesting cities");
    var cities = await db.Cities.ToListAsync();
    op.Complete();
    Log.Information("Fetched {Count} cities", cities.Count);
    return TypedResults.Ok(cities);
  }

  [SwaggerOperation(Summary = "Creates new city")]
  [SwaggerResponse(201, "City was created successfully", typeof(City))]
  [SwaggerResponse(400, "Invalid data was passed", typeof(ProblemDetails))]
  private static async Task<IResult> CreateCityHandler(
    [FromBody] [SwaggerRequestBody(Description = "Name of the city to create", Required = true)] CreateCityRequestBody city,
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<CreateCityRequestBody> validator,
    [FromServices] Mappers mapper
  ) {
    await validator.ValidateAndThrowAsync(city);

    using var op = Operation.Begin("Saving new city");
    var entity = mapper.Map(city);
    await db.Cities.AddAsync(entity);
    await db.SaveChangesAsync();
    op.Complete("City", city.Name);

    Log.Information("Added new city '{Name}'", city.Name);
    return TypedResults.Created("/api/cities", entity);
  }
}