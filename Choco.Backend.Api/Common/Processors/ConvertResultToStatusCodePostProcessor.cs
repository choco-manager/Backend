using Ardalis.Result;
using FastEndpoints;
using IResult = Ardalis.Result.IResult;

namespace Choco.Backend.Api.Common.Processors;

public class ConvertResultToStatusCodePostProcessor : IGlobalPostProcessor
{
    public async Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
    {
        if (!context.HttpContext.ResponseStarted() && !context.HasExceptionOccurred)
        {
            var result = (IResult)context.Response!;
            await context.HttpContext.Response.SendAsync(result, GetResultCode(result.Status), cancellation: ct);
        }
    }

    private static int GetResultCode(ResultStatus status)
    {
        return status switch
        {
            ResultStatus.Ok => 200,
            ResultStatus.Created => 201,
            ResultStatus.NoContent => 204,
            ResultStatus.Error => 500,
            ResultStatus.Forbidden => 403,
            ResultStatus.Unauthorized => 401,
            ResultStatus.Invalid => 400,
            ResultStatus.NotFound => 404,
            ResultStatus.Conflict => 409,
            ResultStatus.CriticalError => 521,
            ResultStatus.Unavailable => 503,
            _ => throw new ArgumentOutOfRangeException(nameof(status), "Unknown status")
        };
    }
}