using System.Security.Claims;
using Api.Common;
using Api.Data;
using Api.Domain.Auth.Data;
using Ardalis.Result;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Auth.UseCases;

public class WhoamiUseCase(AppDbContext db)
    : IUseCase<AuthorizedEmptyRequest, WhoamiResponse>
{
    public async Task<Result<WhoamiResponse>> Execute(AuthorizedEmptyRequest req, CancellationToken ct)
    {
        var login = req.User.ClaimValue(ClaimTypes.Name);
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Login == login, ct);

        if (user is null)
        {
            return Result.Unauthorized();
        }

        return Result.Success(new WhoamiResponse
        {
            Login = user.Login,
            Name = user.Name
        });
    }
}