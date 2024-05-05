using Api.Common;
using Api.Data;
using Api.Domain.Products.Data;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Products.UseCases;

public class GetAllProductsUseCase(AppDbContext db) : IPagedUseCase<PagedRequest, List<ProductDto>>
{
    public async Task<PagedResult<List<ProductDto>>> Execute(PagedRequest res, CancellationToken ct)
    {
        var skip = (res.Page - 1) * res.PageSize;

        var products = await db.Products
            .Skip(skip)
            .Take(res.PageSize)
            .Select(e => ProductMapper.ProductToDto(e))
            .ToListAsync(ct);

        var totalRecords = await db.Products.CountAsync(ct);
        var totalPages = (int)Math.Ceiling((double)totalRecords / res.PageSize);

        return Result<List<ProductDto>>.Success(products).ToPagedResult(new PagedInfo(
            res.Page,
            res.PageSize,
            totalPages, totalRecords));
    }
}