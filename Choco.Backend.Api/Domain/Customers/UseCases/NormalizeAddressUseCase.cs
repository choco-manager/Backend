using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Models;
using Dadata;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Customers.UseCases;

public class NormalizeAddressUseCase(AppDbContext db, DadataConfiguration configuration) : IUseCase<string, Address>
{
    public async Task<Result<Address>> Execute(string req, CancellationToken ct = default)
    {
        var dadata = new CleanClientAsync(configuration.Token, configuration.Secret);
        var dadataAddress = await dadata.Clean<Dadata.Model.Address>(req, ct);

        var city = await db.Cities
            .Where(e => e.Name == dadataAddress.city)
            .FirstOrDefaultAsync(ct);

        if (city is null)
        {
            city = new City
            {
                Name = dadataAddress.city
            };

            await db.Cities.AddAsync(city, ct);
        }

        var address = await db.Addresses
            .Where(e =>
                e.City == city && e.Street == dadataAddress.street_with_type && e.Building == dadataAddress.house)
            .FirstOrDefaultAsync(ct);

        if (address is null)
        {
            address = new Address
            {
                City = city,
                Street = dadataAddress.street_with_type,
                Building = dadataAddress.house
            };

            await db.Addresses.AddAsync(address, ct);
        }

        await db.SaveChangesAsync(ct);

        return Result.Success(address);
    }
}