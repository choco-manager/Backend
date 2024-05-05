using Api.Common;
using Api.Data;
using Api.Domain.Products.Data;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Products.UseCases;

public class GetProductUseCase(AppDbContext db) : IUseCase<IdModel, ProductDto>
{
    public async Task<Result<ProductDto>> Execute(IdModel res, CancellationToken ct = default)
    {
        var product = await db.Products
            .Where(e => e.Id == res.Id)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(ct);

        if (product is null)
        {
            return Result.NotFound(nameof(product));
        }
        
        return Result.Success(ProductMapper.ProductToDto(product));
        
    }
}