using Ardalis.Result;

namespace Choco.Backend.Api.Common;

public interface IPagedUseCase<in TData, TResult>
{
    public Task<PagedResult<TResult>> Execute(TData req, CancellationToken ct);
}