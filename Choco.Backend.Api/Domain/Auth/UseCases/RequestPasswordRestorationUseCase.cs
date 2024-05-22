using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.Utils;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Auth.UseCases;

public class RequestPasswordRestorationUseCase(AppDbContext db, SecurityConfiguration configuration)
    : IUseCase<UsernameRequest, RestorationTokenResponse>
{
    public async Task<Result<RestorationTokenResponse>> Execute(UsernameRequest req, CancellationToken ct = default)
    {
        var user = await db.Users.Where(e => e.Login == req.Login).FirstOrDefaultAsync(ct);

        if (user is null)
        {
            return Result.Forbidden();
        }

        AuthUtils.CreateRestorationToken(configuration, req.Login, out var restorationToken, out var salt);

        var token = new RestorationToken
        {
            Token = restorationToken,
            Salt = salt,
            IsValid = true,
            ValidUntil = DateTime.UtcNow.AddMinutes(3).ToUniversalTime(),
            Login = req.Login
        };

        await db.RestorationTokens.AddAsync(token, ct);
        await db.SaveChangesAsync(ct);

        return Result.Success(
            new RestorationTokenResponse
            {
                RestorationToken = restorationToken,
                Login = req.Login
            }
        );
    }
}