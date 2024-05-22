using System.Net;
using Ardalis.Result;
using FastEndpoints;
using Serilog;

namespace Choco.Backend.Api.Common.Processors;

public class HandleExceptionsPostProcessor : IGlobalPostProcessor
{
    public async Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
    {
        if (!context.HttpContext.ResponseStarted() && context.HasExceptionOccurred)
        {
            var exception = context.ExceptionDispatchInfo!.SourceException;

            var result = Result.Error($"{exception.GetType().Name}: {exception.Message}");
            
            Log.Error(exception, "Exception occured:");
            
            context.MarkExceptionAsHandled();

            await context.HttpContext.Response.SendAsync(result, (int)HttpStatusCode.InternalServerError,
                cancellation: ct);
        }
    }
}