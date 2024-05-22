using Ardalis.Result;

namespace Choco.Backend.Api.Common;

public interface IUseCase<in TData, TResult>
{
    public Task<Result<TResult>> Execute(TData req, CancellationToken ct = default);
}