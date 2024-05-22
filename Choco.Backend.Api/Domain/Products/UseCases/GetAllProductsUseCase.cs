using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.Products.Data;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Products.UseCases;

public class GetAllProductsUseCase(AppDbContext db) : IPagedUseCase<PagedRequest, ListOfProducts>
{
    public async Task<PagedResult<ListOfProducts>> Execute(PagedRequest req, CancellationToken ct)
    {
        var skip = (req.Page - 1) * req.PageSize;

        var products = await db.Products
            .Include(e => e.Tags)
            .Include(e => e.Prices)
            .OrderByDescending(p => p.Prices.OrderByDescending(ph => ph.EffectiveTimestamp).First().EffectiveTimestamp)
            .Skip(skip)
            .Take(req.PageSize)
            .Select(e => ProductMapper
                .ProductToDto(e, e.Prices
                    .OrderByDescending(ph => ph.EffectiveTimestamp)
                    .ToList()
                )
            )
            .ToListAsync(ct);

        var totalRecords = await db.Products.CountAsync(ct);
        var totalPages = (int)Math.Ceiling((double)totalRecords / req.PageSize);

        return Result.Success(new ListOfProducts
            {
                Products = products
            }
        ).ToPagedResult(new PagedInfo(req.Page, req.PageSize, totalPages, totalRecords));
    }
}