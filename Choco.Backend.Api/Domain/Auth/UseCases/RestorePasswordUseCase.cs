using Choco.Backend.Api.Extensions;
using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.Utils;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Auth.UseCases;

public class RestorePasswordUseCase(AppDbContext db, SecurityConfiguration configuration)
    : IUseCase<RestorePasswordRequest, LoginResponse>
{
    public async Task<Result<LoginResponse>> Execute(RestorePasswordRequest req, CancellationToken ct = default)
    {
        var ctx = ValidationContext<RestorePasswordRequest>.Instance;

        if (ctx.ValidationFailed)
        {
            return Result.Invalid(ctx.ValidationFailures.AsErrors());
        }

        var restorationToken = await db.RestorationTokens
            .Where(e => e.Token == req.RestorationToken && e.IsValid && e.ValidUntil >= DateTime.UtcNow)
            .FirstOrDefaultAsync(ct);

        if (restorationToken is null)
        {
            return Result.Forbidden();
        }

        var user = await db.Users.Where(e => e.Login == restorationToken.Login).FirstOrDefaultAsync(ct);

        if (user is null)
        {
            return Result.Forbidden();
        }

        AuthUtils.CreatePasswordHash(req.NewPassword, out var passwordSalt, out var passwordHash);
        AuthUtils.CreateAccessToken(configuration, user, out var accessToken);
        AuthUtils.CreateRefreshToken(configuration, user, out var refreshToken, out var salt);

        restorationToken.IsValid = false;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        var refreshTokens = await db.RefreshTokens.Where(e => e.UserId == user.Id).ToListAsync(ct);

        db.RefreshTokens.RemoveRange(refreshTokens);

        var rt = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            Salt = salt,
            ExpireAt = DateTime.UtcNow.AddDays(14)
        };

        await db.RefreshTokens.AddAsync(rt, ct);

        await db.SaveChangesAsync(ct);

        return Result.Success(new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }
}