using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.ProductTags.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.ProductTags.UseCases;

public class GetAllProductTagsUseCase(AppDbContext db) : IUseCase<EmptyRequest, ProductTagsResponse>
{
    public async Task<Result<ProductTagsResponse>> Execute(EmptyRequest req, CancellationToken ct = default)
    {
        var tags = await db.ProductTags.ToListAsync(ct);

        return Result.Success(
            new ProductTagsResponse
            {
                Tags = tags,
            }
        );
    }
}