using System.Security.Claims;
using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.Auth.Data;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Auth.UseCases;

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