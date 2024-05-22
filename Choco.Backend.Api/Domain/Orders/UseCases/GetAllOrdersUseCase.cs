﻿using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Domain.Orders.Data;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Orders.UseCases;

public class GetAllOrdersUseCase(AppDbContext db) : IPagedUseCase<PagedRequest, ListOfOrders>
{
    public async Task<PagedResult<ListOfOrders>> Execute(PagedRequest req, CancellationToken ct)
    {
        var orders = await db.Orders
            .Include(e => e.Products)
            .ThenInclude(e => e.Product)
            .Skip(req.Skip)
            .Take(req.PageSize)
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
            .ToListAsync(ct);

        var totalRecords = await db.Orders.CountAsync(ct);
        var totalPages = (int)Math.Ceiling((double)totalRecords / req.PageSize);

        return Result.Success(new ListOfOrders
        {
            Orders = orders
        }).ToPagedResult(new PagedInfo(req.Page, req.PageSize, totalPages, totalRecords));
    }
}