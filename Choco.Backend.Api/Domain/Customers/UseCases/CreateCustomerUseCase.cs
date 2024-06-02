using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Customers.Data;

namespace Choco.Backend.Api.Domain.Customers.UseCases;

public class CreateCustomerUseCase(AppDbContext db, NormalizeAddressUseCase normalizeAddressUseCase)
    : IUseCase<CreateCustomerRequest, IdModel>
{
    public async Task<Result<IdModel>> Execute(CreateCustomerRequest req, CancellationToken ct = default)
    {
        List<Address> addresses = [];

        foreach (var address in req.DeliveryAddresses)
        {
            var result = await normalizeAddressUseCase.Execute(address, ct);

            if (result.IsSuccess)
            {
                addresses.Add(result.Value);
            }
        }

        var customer = new Customer
        {
            Name = req.Name,
            ShippingAddresses = addresses,
            PhoneNumber = req.PhoneNumber,
            VkId = req.VkId
        };

        await db.Customers.AddAsync(customer, ct);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}