using System.Net;
using System.Net.Mime;
using System.Text.Json;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

using Serilog;


namespace Backend.Middlewares;

public class GlobalErrorHandlingMiddleware : IMiddleware {
  public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
    try
    {
      await next(context);
    }
    catch (ValidationException e)
    {
      Log.Warning("Validation was failed with errors: {Errors}", e.Errors);

      var problemDetails = new ProblemDetails {
        Status = (int)HttpStatusCode.BadRequest,
        Title = "Failed validation",
        Detail = string.Join(", ",
          e.Errors.Select(failure =>
            $"Error in {failure.PropertyName}: {failure.ErrorMessage} (passed value '{failure.AttemptedValue}')")),
      };

      var json = JsonSerializer.Serialize(problemDetails);
      context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      context.Response.ContentType = MediaTypeNames.Application.Json;
      await context.Response.WriteAsync(json);
    }
    catch (Exception e)
    {
      Log.Error(
        "Unexpected {ExceptionClass} happened: {Message}",
        e.GetType().Name,
        e.Message
      );

      var problemDetails = new ProblemDetails {
        Status = (int)HttpStatusCode.InternalServerError,
        Title = e.GetType().Name,
        Detail = e.Message,
      };

      var json = JsonSerializer.Serialize(problemDetails);

      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      context.Response.ContentType = MediaTypeNames.Application.Json;
      await context.Response.WriteAsync(json);
    }
  }
}