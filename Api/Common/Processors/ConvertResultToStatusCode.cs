using Ardalis.Result;
using FastEndpoints;
using IResult = Ardalis.Result.IResult;

namespace Api.Common.Processors;

public class ConvertResultToStatusCode: IGlobalPostProcessor
{
    public async Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
    {
        if (!context.HttpContext.ResponseStarted()){
            var statusCode = 200;
            var result = (IResult)context.Response!;
            switch (result.Status)
            {
                case ResultStatus.Ok:
                {
                    break;
                }
                case ResultStatus.Error:
                {
                    statusCode = 500;
                    break;
                }
                case ResultStatus.Forbidden:
                {
                    statusCode = 403;
                    break;
                }
                case ResultStatus.Unauthorized:
                {
                    statusCode = 401;
                    break;
                }
                case ResultStatus.Invalid:
                {
                    statusCode = 400;
                    break;
                }
                case ResultStatus.NotFound:
                {
                    statusCode = 404;
                    break;
                }
                case ResultStatus.Conflict:
                {
                    statusCode = 409;
                    break;
                }
                case ResultStatus.CriticalError:
                {
                    statusCode = 521;
                    break;
                }
                case ResultStatus.Unavailable:
                {
                    statusCode = 503;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), "Unknown status");
            }
            await context.HttpContext.Response.SendAsync(result, statusCode, cancellation: ct);
        }
    }
}