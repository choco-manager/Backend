using System.Security.Claims;
using Ardalis.Result;

namespace Choco.Backend.Api.Common;

public interface IAuthorizedUseCase<in TData, TResult>
{
    public Task<Result<TResult>> Execute(ClaimsPrincipal user, TData req, CancellationToken ct = default);
}