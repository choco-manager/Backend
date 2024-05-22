using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.Utils;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Auth.UseCases;

public class RefreshUseCase(AppDbContext db, SecurityConfiguration configuration)
    : IUseCase<RefreshRequest, LoginResponse>
{
    public async Task<Result<LoginResponse>> Execute(RefreshRequest req, CancellationToken ct)
    {
        var rt = await db.RefreshTokens
            .Where(e => e.Token == req.RefreshToken && e.ExpireAt >= DateTime.UtcNow)
            .FirstOrDefaultAsync(ct);

        if (rt is null)
        {
            return Result.Forbidden();
        }

        var user = await db.Users
            .Where(e => e.Id == rt.UserId)
            .FirstOrDefaultAsync(ct);

        if (user is null)
        {
            return Result.Forbidden();
        }

        AuthUtils.CreateAccessToken(configuration, user, out var accessToken);
        AuthUtils.CreateRefreshToken(configuration, user, out var refreshToken, out var refreshTokenSalt);

        rt.Token = refreshToken;
        rt.Salt = refreshTokenSalt;
        rt.ExpireAt = DateTime.UtcNow.AddDays(14);

        await db.SaveChangesAsync(ct);

        return Result.Success(new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }
}