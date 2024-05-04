using Api.Common;
using Api.Configuration;
using Api.Data;
using Api.Data.Models;
using Api.Domain.Auth.Data;
using Api.Domain.Auth.Utils;
using Api.Extensions;
using Ardalis.Result;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Auth.UseCases;

public class LoginUseCase(AppDbContext db, SecurityConfiguration configuration) : IUseCase<LoginRequest, LoginResponse>
{
    public async Task<Result<LoginResponse>> Execute(LoginRequest res, CancellationToken ct)
    {
        var ctx = ValidationContext<RegisterRequest>.Instance;

        if (ctx.ValidationFailed)
        {
            return Result.Invalid(ctx.ValidationFailures.AsErrors());
        }

        var user = await db.Users
            .AsNoTracking()
            .Where(e => e.Login == res.Login)
            .FirstOrDefaultAsync(ct);

        if (user is null || !AuthUtils.IsValidPassword(user, res))
        {
            return Result<LoginResponse>.Unauthorized();
        }

        AuthUtils.CreateRefreshToken(configuration, user, out var refreshToken, out var refreshTokenSalt);
        AuthUtils.CreateAccessToken(configuration, user, out var accessToken);

        var rt = await db.RefreshTokens.Where(e => e.UserId == user.Id).FirstOrDefaultAsync(ct);

        if (rt is null)
        {
            rt = new RefreshToken
            {
                ExpireAt = DateTime.UtcNow.AddDays(14),
                Token = refreshToken,
                Salt = refreshTokenSalt,
                UserId = user.Id
            };
            await db.RefreshTokens.AddAsync(rt, ct);
        }
        else
        {
            rt.Token = refreshToken;
            rt.Salt = refreshTokenSalt;
            rt.ExpireAt = DateTime.UtcNow.AddDays(14);
        }

        await db.SaveChangesAsync(ct);

        return Result<LoginResponse>.Success(new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }
}