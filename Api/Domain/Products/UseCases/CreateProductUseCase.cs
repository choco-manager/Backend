using Api.Common;
using Api.Data;
using Api.Data.Models;
using Api.Domain.Products.Data;
using Api.Extensions;
using Ardalis.Result;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Products.UseCases;

public class CreateProductUseCase(AppDbContext db) : IUseCase<CreateProductRequest, ProductDto>
{
    public async Task<Result<ProductDto>> Execute(CreateProductRequest res, CancellationToken ct)
    {
        var ctx = ValidationContext<CreateProductRequest>.Instance;

        if (ctx.ValidationFailed)
        {
            return Result.Invalid(ctx.ValidationFailures.AsErrors());
        }

        List<ProductTag> tags = [];

        foreach (var tagId in res.Tags)
        {
            var tag = await db.ProductTags.Where(e => e.Id == tagId).FirstOrDefaultAsync(ct);

            if (tag is not null)
            {
                tags.Add(tag);
            }
        }

        var product = new Product
        {
            Title = res.Title,
            CostPrice = res.CostPrice,
            RetailPrice = res.RetailPrice,
            StockBalance = res.StockBalance,
            IsBulk = res.IsBulk,
            Tags = tags
        };

        await db.Products.AddAsync(product, ct);
        await db.SaveChangesAsync(ct);

        return Result.Success(ProductMapper.ProductToDto(product));
    }
}