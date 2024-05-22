using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Domain.Orders.Data;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Orders.UseCases;

public class GetOrderUseCase(AppDbContext db) : IUseCase<IdModel, OrderDto>
{
    public async Task<Result<OrderDto>> Execute(IdModel req, CancellationToken ct = default)
    {
        var order = await db.Orders
            .Where(e => e.Id == req.Id)
            .Include(e => e.Products)
            .ThenInclude(e => e.Product)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderedAt = o.OrderedAt,
                ToBeDeliveredAt = o.ToBeDeliveredAt,
                OrderStatus = o.OrderStatus,
                PaymentStatus = o.PaymentStatus,
                ShippingAddress = o.ShippingAddress,
                TotalAmount = o.TotalAmount,
                Products = o.Products.Select(op => new OrderedProductDto
                {
                    ProductName = op.Product.Title,
                    Amount = op.Amount,
                    Price = db.PriceHistory
                        .Where(ph =>
                            ph.ProductId == op.ProductId && ph.EffectiveTimestamp <= o.OrderedAt &&
                            ph.PriceType == PriceType.Retail)
                        .OrderByDescending(ph => ph.EffectiveTimestamp)
                        .FirstOrDefault()!.Price
                }).ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (order is null)
        {
            return Result.NotFound(nameof(order));
        }

        return Result.Success(order);
    }
}