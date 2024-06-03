using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Procurement.Data;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Procurement.UseCases;

public class CreateProcurementUseCase(AppDbContext db) : IUseCase<CreateProcurementRequest, IdModel>
{
    public async Task<Result<IdModel>> Execute(CreateProcurementRequest req, CancellationToken ct = default)
    {
        List<ProcuredProduct> products = [];

        foreach (var item in req.Products)
        {
            var data = await db.Products
                .Where(e => e.Id == item.Product)
                .FirstOrDefaultAsync(ct);

            if (data is not null)
            {
                products.Add(new ProcuredProduct
                {
                    Product = data,
                    Amount = item.Amount
                });
            }
        }

        var procurement = new Api.Data.Models.Procurement
        {
            Products = products,
            Status = OrderStatus.Pending,
            IsDeleted = false,
            ProcuredAt = DateTime.UtcNow.ToUniversalTime(),
        };

        await db.Procurements.AddAsync(procurement, ct);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}