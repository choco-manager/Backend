using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.Customers.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Customers.UseCases;

public class GetAllCustomersUseCase(AppDbContext db) : IUseCase<EmptyRequest, ListOfCustomers>
{
    public async Task<Result<ListOfCustomers>> Execute(EmptyRequest req, CancellationToken ct = default)
    {
        var customers = await db.Customers
            .Include(e => e.ShippingAddresses)
            .ThenInclude(e => e.City)
            .Select(e => new CustomerDto
            {
                Name = e.Name,
                ShippingAddresses = e.ShippingAddresses.Select(a => new AddressDto
                {
                    Id = a.Id,
                    Address = a.ToString(),
                }).ToList()
            }).ToListAsync(ct);

        return Result.Success(new ListOfCustomers { Customers = customers });
    }
}