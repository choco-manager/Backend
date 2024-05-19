using Api.Common;
using Api.Data;
using Api.Domain.ProductTags.Data;
using Ardalis.Result;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.ProductTags.UseCases;

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