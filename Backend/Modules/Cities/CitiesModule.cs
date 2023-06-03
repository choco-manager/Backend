using Backend.Data;
using Backend.Modules.Cities.Contract;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules.Cities;

public class CitiesModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection builder)
    {
        builder.AddSingleton<AbstractValidator<CityRequestBody>, CityRequestBodyValidator>();
        builder.AddSingleton<Mappers>();
        return builder;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/cities", GetCitiesHandler);
        endpoints.MapPost("/api/cities", CreateCityHandler);

        return endpoints;
    }

    private static async Task<IResult> GetCitiesHandler([FromServices] ApplicationDbContext db)
    {
        return TypedResults.Ok(await db.Cities.ToListAsync());
    }

    private static async Task<IResult> CreateCityHandler([FromBody] CityRequestBody city, [FromServices] ApplicationDbContext db,
        [FromServices] AbstractValidator<CityRequestBody> validator, [FromServices] Mappers mapper)
    {
        var result = await validator.ValidateAsync(city);

        if (!result.IsValid) return TypedResults.BadRequest(result.Errors);

        await db.Cities.AddAsync(mapper.Map(city));
        await db.SaveChangesAsync();
        return TypedResults.Created("/api/cities");
    }
}