using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.Products.Data;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Products.UseCases;

public class GetProductUseCase(AppDbContext db) : IUseCase<IdModel, ProductDto>
{
    public async Task<Result<ProductDto>> Execute(IdModel req, CancellationToken ct = default)
    {
        var product = await db.Products
            .Where(e => e.Id == req.Id)
            .Include(e => e.Tags)
            .Include(e => e.Prices)
            .FirstOrDefaultAsync(ct);

        if (product is null)
        {
            return Result.NotFound(nameof(product));
        }

        return Result.Success(ProductMapper.ProductToDto(product,
            product.Prices.OrderByDescending(e => e.EffectiveTimestamp).ToList()));
    }
}