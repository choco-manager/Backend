using Api.Common;
using Api.Data;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Auth.UseCases;

public class ValidateTokenUseCase(AppDbContext db) : IUseCase<string?, bool>
{
    public async Task<Result<bool>> Execute(string? req, CancellationToken ct = default)
    {
        if (req is null)
        {
            return Result.Success(false);
        }
        
        return Result.Success(await db.RevokedAccessTokens.AnyAsync(e => e.Token == req, ct));
    }
}