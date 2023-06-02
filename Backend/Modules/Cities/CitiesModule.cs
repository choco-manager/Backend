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
        builder.AddSingleton<AbstractValidator<City>, CityValidator>();
        return builder;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/api/cities",
            async (ApplicationDbContext db) => TypedResults.Ok(await db.Cities.ToListAsync())
        );

        endpoints.MapPost("/api/cities", CreateCityHandler);

        return endpoints;
    }

    private async Task<IResult> CreateCityHandler([FromBody] City city, ApplicationDbContext db,
        AbstractValidator<City> validator)
    {
        var result = await validator.ValidateAsync(city);

        if (!result.IsValid) return TypedResults.BadRequest(result.Errors);

        await db.Cities.AddAsync(city);
        return TypedResults.Created("/api/cities");
    }
}