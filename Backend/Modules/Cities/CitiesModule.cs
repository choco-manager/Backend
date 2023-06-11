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
using Backend.Modules.Cities.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.Cities;

public class CitiesModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<AbstractValidator<CreateCityRequestBody>, CityRequestBodyValidator>();
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/api/cities", GetCitiesHandler).WithTags("Cities API");
    endpoints.MapPost("/api/cities", CreateCityHandler).WithTags("Cities API");

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
    [FromBody] [SwaggerRequestBody(Description = "Name of the city to create", Required = true)]
    CreateCityRequestBody city,
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<CreateCityRequestBody> validator,
    [FromServices] Mappers mapper
  ) {
    await validator.ValidateAndThrowAsync(city);

    using var op = Operation.Begin("Saving new city");
    var entity = mapper.Enhance(city);
    await db.Cities.AddAsync(entity);
    await db.SaveChangesAsync();
    op.Complete("City", city.Name);

    Log.Information("Added new city '{Name}'", city.Name);
    return TypedResults.Created("/api/cities", entity);
  }
}