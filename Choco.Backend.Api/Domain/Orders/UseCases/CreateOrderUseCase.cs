using System.Security.Claims;
using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Notifications.Data;
using Choco.Backend.Api.Domain.Notifications.UseCases;
using Choco.Backend.Api.Domain.Orders.Data;
using FastEndpoints.Security;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Orders.UseCases;

public class CreateOrderUseCase(
    AppDbContext db,
    CreateNotificationUseCase createNotificationUseCase,
    IBackgroundJobClient jobClient)
    : IAuthorizedUseCase<CreateOrderRequest, IdModel>
{
    public async Task<Result<IdModel>> Execute(ClaimsPrincipal user, CreateOrderRequest req,
        CancellationToken ct = default)
    {
        var userLogin = user.ClaimValue(ClaimTypes.Name);

        if (userLogin is null)
        {
            return Result.Unauthorized();
        }

        var allUsers = await db.Users
            .Select(e => e.Id)
            .ToListAsync(ct);

        var customer = await db.Customers
            .Where(e => e.Id == req.CustomerId)
            .FirstOrDefaultAsync(ct);

        if (customer is null)
        {
            return Result.NotFound(nameof(customer));
        }

        var order = new Order
        {
            Products = [],
            Customer = customer,
            OrderedAt = DateTime.UtcNow.ToUniversalTime(),
            OrderStatus = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            ToBeDeliveredAt = req.ToBeDeliveredAt
        };

        await db.Orders.AddAsync(order, ct);
        List<OrderedProduct> products = [];

        var subTotal = 0m;

        foreach (var productRequest in req.Products)
        {
            var data = await db.Products
                .Where(e => e.Id == productRequest.Product)
                .Include(e => e.Prices)
                .FirstOrDefaultAsync(ct);

            if (data is null) continue;

            data.StockBalance -= productRequest.Amount;

            var price = data.Prices
                .Where(e => e.PriceType == PriceType.Retail)
                .OrderByDescending(ph => ph.EffectiveTimestamp)
                .First();

            var product = new OrderedProduct
            {
                Order = order,
                Product = data,
                Amount = productRequest.Amount
            };

            products.Add(product);
            subTotal += price * productRequest.Amount;
        }

        order.Products = products;
        order.TotalAmount = subTotal;

        var newOrderNotificationData = new NotificationData
        {
            Notification = new Notification
            {
                Title = "Новый заказ",
                Timestamp = DateTime.UtcNow.ToUniversalTime(),
                NotificationType = NotificationType.OrderNew,
                TriggerId = order.Id,
            },
            Recipients = allUsers
        };
        var orderDeliverySoonNotificationData = new NotificationData
        {
            Notification = new Notification
            {
                Title = "Приближается срок доставки заказа",
                Timestamp = (order.ToBeDeliveredAt - DateTime.UtcNow).TotalHours > 2
                    ? order.ToBeDeliveredAt.AddHours(-2).ToUniversalTime()
                    : DateTime.UtcNow.ToUniversalTime(),
                NotificationType = NotificationType.OrderDeliverySoon,
                TriggerId = order.Id,
            },
            Recipients = allUsers
        };

        jobClient.Enqueue(() => createNotificationUseCase.Execute(newOrderNotificationData, ct));
        jobClient.Enqueue(() => createNotificationUseCase.Execute(orderDeliverySoonNotificationData, ct));

        await db.SaveChangesAsync(ct);

        return Result<IdModel>.Created(new IdModel { Id = order.Id }, "/orders");
    }
}