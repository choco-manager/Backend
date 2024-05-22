using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.Stocktake.Data;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Stocktake.UseCases;

public class DoStocktakeUseCase(AppDbContext db) : IUseCase<DoStocktakeRequest, DoStocktakeResponse>
{
    public async Task<Result<DoStocktakeResponse>> Execute(DoStocktakeRequest req, CancellationToken ct = default)
    {
        var product = await db.Products.Where(e => e.Id == req.ProductId).FirstOrDefaultAsync(ct);

        if (product is null)
        {
            return Result.NotFound(nameof(product));
        }

        var delta = req.StockBalance - product.StockBalance;

        product.StockBalance += delta;

        await db.SaveChangesAsync(ct);

        return Result.Success(new DoStocktakeResponse
        {
            ProductTitle = product.Title,
            StockBalanceDelta = delta
        });
    }
}