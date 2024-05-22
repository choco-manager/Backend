using Choco.Backend.Api.Extensions;
using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.Utils;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.UseCases;

public class RegisterUseCase(AppDbContext db, SecurityConfiguration configuration)
    : IUseCase<RegisterRequest, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Execute(RegisterRequest req, CancellationToken ct)
    {
        var ctx = ValidationContext<RegisterRequest>.Instance;

        if (ctx.ValidationFailed)
        {
            return Result.Invalid(ctx.ValidationFailures.AsErrors());
        }

        AuthUtils.CreatePasswordHash(req.Password, out var passwordSalt, out var passwordHash);

        var user = new User
        {
            Login = req.Login,
            Name = req.Name,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        AuthUtils.CreateRefreshToken(configuration, user, out var refreshToken, out var refreshTokenSalt);
        AuthUtils.CreateAccessToken(configuration, user, out var accessToken);

        var rt = new RefreshToken
        {
            ExpireAt = DateTime.UtcNow.AddDays(14),
            Token = refreshToken,
            Salt = refreshTokenSalt,
            UserId = user.Id
        };

        await db.Users.AddAsync(user, ct);
        await db.RefreshTokens.AddAsync(rt, ct);

        await db.SaveChangesAsync(ct);

        return Result.Success(new RegisterResponse
        {
            Login = req.Login,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }
}