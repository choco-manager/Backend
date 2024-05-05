using Api.Common;
using Api.Data;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Auth.UseCases;

public class ValidateTokenUseCase(AppDbContext db) : IUseCase<string?, bool>
{
    public async Task<Result<bool>> Execute(string? res, CancellationToken ct = default)
    {
        if (res is null)
        {
            return Result.Success(false);
        }
        
        return Result.Success(await db.RevokedAccessTokens.AnyAsync(e => e.Token == res, ct));
    }
}