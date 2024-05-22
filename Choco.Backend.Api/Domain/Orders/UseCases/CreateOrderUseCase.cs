using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Orders.Data;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Orders.UseCases;

public class CreateOrderUseCase(AppDbContext db): IUseCase<CreateOrderRequest, IdModel>
{
    public async Task<Result<IdModel>> Execute(CreateOrderRequest req, CancellationToken ct = default)
    {
        var order = new Order
        {
            Products = [],
            ShippingAddress = req.ShippingAddress,
            OrderedAt = DateTime.UtcNow.ToUniversalTime(),
            OrderStatus = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            ToBeDeliveredAt = req.ToBeDeliveredAt
        };

        await db.Orders.AddAsync(order, ct);
        List<OrderedProduct> products = [];

        foreach (var productRequest in req.Products)
        {
            var data = await db.Products
                .Where(e => e.Id == productRequest.Product)
                .FirstOrDefaultAsync(ct);

            if (data is null) continue;
            
            var product = new OrderedProduct
            {
                Order = order,
                Product = data,
                Amount = productRequest.Amount
            };
                
            products.Add(product);
        }

        order.Products = products;

        await db.SaveChangesAsync(ct);

        return Result<IdModel>.Created(new IdModel { Id = order.Id }, "/orders");
    }
}