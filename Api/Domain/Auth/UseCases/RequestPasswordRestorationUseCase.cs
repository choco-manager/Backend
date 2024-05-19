using Api.Common;
using Api.Configuration;
using Api.Data;
using Api.Data.Models;
using Api.Domain.Auth.Data;
using Api.Domain.Auth.Utils;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Auth.UseCases;

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
            IsValid = true
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