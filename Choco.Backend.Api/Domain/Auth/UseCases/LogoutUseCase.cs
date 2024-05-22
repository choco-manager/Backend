using System.Security.Claims;
using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Auth.Data;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Auth.UseCases;

public class LogoutUseCase(AppDbContext db) : IUseCase<AuthorizedLogoutRequest, EmptyResponse>
{
    public async Task<Result<EmptyResponse>> Execute(AuthorizedLogoutRequest req, CancellationToken ct)
    {
        var login = req.User.ClaimValue(ClaimTypes.Name);
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Login == login, ct);

        if (user is null)
        {
            return Result.Unauthorized();
        }

        var rt = await db.RefreshTokens
            .Where(e => e.UserId == user.Id)
            .FirstOrDefaultAsync(ct);

        if (rt is not null)
        {
            db.RefreshTokens.Remove(rt);
        }

        var rat = await db.RevokedAccessTokens
            .Where(e => e.Token == req.AccessToken)
            .FirstOrDefaultAsync(ct);

        if (rat is null)
        {
            await db.RevokedAccessTokens.AddAsync(new RevokedAccessToken
            {
                Token = req.AccessToken
            }, ct);
        }

        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}