using Api.Common;
using Api.Data;
using Api.Data.Enums;
using Api.Data.Models;
using Api.Domain.Products.Data;
using Api.Extensions;
using Ardalis.Result;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Products.UseCases;

public class CreateProductUseCase(AppDbContext db) : IUseCase<CreateProductRequest, ProductDto>
{
    public async Task<Result<ProductDto>> Execute(CreateProductRequest req, CancellationToken ct)
    {
        var ctx = ValidationContext<CreateProductRequest>.Instance;

        if (ctx.ValidationFailed)
        {
            return Result.Invalid(ctx.ValidationFailures.AsErrors());
        }

        List<ProductTag> tags = [];

        foreach (var tagId in req.Tags)
        {
            var tag = await db.ProductTags.Where(e => e.Id == tagId).FirstOrDefaultAsync(ct);

            if (tag is not null)
            {
                tags.Add(tag);
            }
        }

        var product = new Product
        {
            Title = req.Title,
            StockBalance = req.StockBalance,
            IsBulk = req.IsBulk,
            Tags = tags,
            IsDeleted = false
        };

        await db.Products.AddAsync(product, ct);

        var retailPrice = new PriceHistory
        {
            ProductId = product.Id,
            Price = req.RetailPrice,
            EffectiveTimestamp = DateTime.UtcNow.ToUniversalTime(),
            PriceType = PriceType.Retail
        };

        var costPrice = new PriceHistory
        {
            ProductId = product.Id,
            Price = req.CostPrice,
            EffectiveTimestamp = DateTime.UtcNow.ToUniversalTime(),
            PriceType = PriceType.Cost
        };

        await db.PriceHistory.AddRangeAsync(retailPrice, costPrice);
        await db.SaveChangesAsync(ct);

        return Result<ProductDto>.Created(ProductMapper.ProductToDto(product, [retailPrice, costPrice]), "/products");
    }
}