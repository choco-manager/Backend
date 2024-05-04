using Ardalis.Result;

namespace Api.Common;

public interface IUseCase<in TData, TResult>
{
    public Task<Result<TResult>> Execute(TData res, CancellationToken ct);
}