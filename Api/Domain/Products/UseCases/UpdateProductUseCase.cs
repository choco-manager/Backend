using Api.Common;
using Api.Data;
using Api.Data.Models;
using Api.Domain.Products.Data;
using Api.Extensions;
using Ardalis.Result;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Products.UseCases;

public class UpdateProductUseCase(AppDbContext db) : IUseCase<UpdateProductRequest, ProductDto>
{
    public async Task<Result<ProductDto>> Execute(UpdateProductRequest res, CancellationToken ct = default)
    {
        var ctx = ValidationContext<UpdateProductRequest>.Instance;

        if (ctx.ValidationFailed)
        {
            return Result.Invalid(ctx.ValidationFailures.AsErrors());
        }

        var product = await db.Products.Where(e => e.Id == res.Id).FirstOrDefaultAsync(ct);

        if (product is null)
        {
            return Result.NotFound(nameof(product));
        }

        List<ProductTag> tags = [];

        if (res.Tags is not null)
        {
            foreach (var tagId in res.Tags)
            {
                var tag = await db.ProductTags.Where(e => e.Id == tagId).FirstOrDefaultAsync(ct);

                if (tag is not null)
                {
                    tags.Add(tag);
                }
            }
        }

        if (res.Title is not null)
        {
            product.Title = res.Title;
        }

        if (res.CostPrice is not null)
        {
            product.CostPrice = res.CostPrice.Value;
        }

        if (res.RetailPrice is not null)
        {
            product.RetailPrice = res.RetailPrice.Value;
        }

        if (res.Tags is not null)
        {
            product.Tags = tags;
        }


        if (res.IsBulk is not null)
        {
            product.IsBulk = res.IsBulk.Value;
        }

        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}