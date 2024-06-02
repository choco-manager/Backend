using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Orders.UseCases;

public class ForceDeleteOrderUseCase(AppDbContext db) : IUseCase<IdModel, EmptyResponse>
{
    public async Task<Result<EmptyResponse>> Execute(IdModel req, CancellationToken ct = default)
    {
        var order = await db.Orders
            .Where(e => e.Id == req.Id)
            .FirstOrDefaultAsync(ct);

        if (order is null)
        {
            return Result.NotFound(nameof(order));
        }

        if (!order.IsDeleted)
        {
            var orderItems = await db.OrderedProducts
                .Where(e => e.OrderId == order.Id)
                .ToListAsync(ct);

            foreach (var item in orderItems)
            {
                var product = await db.Products
                    .Where(e => e.Id == item.ProductId)
                    .FirstOrDefaultAsync(ct);

                if (product is not null)
                {
                    product.StockBalance += item.Amount;
                }
            }
        }

        db.Orders.Remove(order);
        await db.SaveChangesAsync(ct);

        return Result.NoContent();
    }
}