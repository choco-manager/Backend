﻿using Choco.Backend.Api.Extensions;
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

public class LoginUseCase(AppDbContext db, SecurityConfiguration configuration) : IUseCase<LoginRequest, LoginResponse>
{
    public async Task<Result<LoginResponse>> Execute(LoginRequest req, CancellationToken ct)
    {
        var ctx = ValidationContext<RegisterRequest>.Instance;

        if (ctx.ValidationFailed)
        {
            return Result.Invalid(ctx.ValidationFailures.AsErrors());
        }

        var user = await db.Users
            .AsNoTracking()
            .Where(e => e.Login == req.Login)
            .FirstOrDefaultAsync(ct);

        if (user is null || !AuthUtils.IsValidPassword(user, req))
        {
            return Result<LoginResponse>.Unauthorized();
        }

        var fcm = await db.FcmTokens.Where(e => e.UserId == user.Id).FirstOrDefaultAsync(ct);

        if (fcm is null)
        {
            fcm = new FcmToken
            {
                Token = req.FcmToken,
                UserId = user.Id
            };
            
            await db.FcmTokens.AddAsync(fcm, ct);
        }
        else
        {
            fcm.Token = req.FcmToken;
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