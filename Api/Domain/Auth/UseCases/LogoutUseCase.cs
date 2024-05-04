using System.Security.Claims;
using Api.Common;
using Api.Data;
using Api.Data.Models;
using Api.Domain.Auth.Data;
using Ardalis.Result;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Auth.UseCases;

public class LogoutUseCase(AppDbContext db): IUseCase<AuthorizedLogoutRequest, EmptyResponse>
{
    public async Task<Result<EmptyResponse>> Execute(AuthorizedLogoutRequest res, CancellationToken ct)
    {
        
        var login = res.User.ClaimValue(ClaimTypes.Name);
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

        await db.RevokedAccessTokens.AddAsync(new RevokedAccessToken
        {
            Token = res.AccessToken
        }, ct);

        await db.SaveChangesAsync(ct);
        
        return Result.Success();
    }
}
