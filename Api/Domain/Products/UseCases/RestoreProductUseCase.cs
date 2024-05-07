using Api.Common;
using Api.Data;
using Api.Domain.Products.Data;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Products.UseCases;

public class RestoreProductUseCase(AppDbContext db): IUseCase<IdModel, ProductDto>
{
    public async Task<Result<ProductDto>> Execute(IdModel req, CancellationToken ct = default)
    {
        var product = await db.Products.Where(e => e.Id == req.Id).FirstOrDefaultAsync(ct);

        if (product is null)
        {
            return Result.NotFound(nameof(product));
        }

        product.IsDeleted = false;

        await db.SaveChangesAsync(ct);
        
        return Result.Success(ProductMapper.ProductToDto(product));

    }
}