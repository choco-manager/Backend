using Choco.Backend.Api.Extensions;
using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Products.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Products.UseCases;

public class UpdateProductUseCase(AppDbContext db) : IUseCase<UpdateProductRequest, ProductDto>
{
    public async Task<Result<ProductDto>> Execute(UpdateProductRequest req, CancellationToken ct = default)
    {
        var ctx = ValidationContext<UpdateProductRequest>.Instance;

        if (ctx.ValidationFailed)
        {
            return Result.Invalid(ctx.ValidationFailures.AsErrors());
        }

        var product = await db.Products.Where(e => e.Id == req.Id).FirstOrDefaultAsync(ct);

        if (product is null)
        {
            return Result.NotFound(nameof(product));
        }

        List<ProductTag> tags = [];

        if (req.Tags is not null)
        {
            foreach (var tagId in req.Tags)
            {
                var tag = await db.ProductTags.Where(e => e.Id == tagId).FirstOrDefaultAsync(ct);

                if (tag is not null)
                {
                    tags.Add(tag);
                }
            }
        }

        if (req.Title is not null)
        {
            product.Title = req.Title;
        }

        if (req.CostPrice is not null)
        {
            var costPrice = new PriceHistory
            {
                ProductId = product.Id,
                Price = req.CostPrice!.Value,
                EffectiveTimestamp = DateTime.UtcNow.ToUniversalTime(),
                PriceType = PriceType.Cost
            };
            await db.PriceHistory.AddAsync(costPrice, ct);

        }

        if (req.RetailPrice is not null)
        {
            var retailPrice = new PriceHistory
            {
                ProductId = product.Id,
                Price = req.RetailPrice!.Value,
                EffectiveTimestamp = DateTime.UtcNow.ToUniversalTime(),
                PriceType = PriceType.Retail
            };
            await db.PriceHistory.AddAsync(retailPrice, ct);
        }

        if (req.Tags is not null)
        {
            product.Tags = tags;
        }


        if (req.IsBulk is not null)
        {
            product.IsBulk = req.IsBulk.Value;
        }

        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}