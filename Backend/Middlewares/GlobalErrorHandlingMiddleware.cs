// ------------------------------------------------------------------------
// Copyright (C) 2023 dadyarri
// This file is part of ChocoManager <https://github.com/choco-manager/Backend>.
// 
// ChocoManager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// ChocoManager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with ChocoManager.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------
// 

#region

using System.Net;
using System.Net.Mime;
using System.Text.Json;

using Backend.Exceptions;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

using Serilog;

#endregion


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
    catch (EntityWasNotFoundException e)
    {
      var title = $"{e.Name} was not found";
      string detail;

      if (e.Id is not null)
      {
        Log.Information("{Entity} with Id = {Id} was not found", e.Name, e.Id);
        detail = $"{e.Name} with Id = {e.Id} is not presented in the database";
      }
      else
      {
        Log.Information("{Entity} by query {Query} was not found", e.Name, e.Query);
        detail = $"{e.Name} with Id = {e.Id} is not presented in the database";
      }

      var problemDetails = new ProblemDetails {
        Status = (int)HttpStatusCode.NotFound,
        Title = title,
        Detail = detail,
      };

      var json = JsonSerializer.Serialize(problemDetails);

      context.Response.StatusCode = (int)HttpStatusCode.NotFound;
      context.Response.ContentType = MediaTypeNames.Application.Json;
      await context.Response.WriteAsync(json);
    }
    catch (CouldNotChangeStatusException e)
    {
      Log.Information(
        "Could not change status from {OldStatus} to {NewStatus} in {MovementType} with Id = {Id}",
        e.OldStatus.Name,
        e.NewStatus.Name,
        e.MovementType,
        e.Id
      );

      var problemDetails = new ProblemDetails {
        Status = (int)HttpStatusCode.Conflict,
        Title = $"Could not change status {e.OldStatus.Name} -> {e.NewStatus.Name}",
        Detail = $"Could not change status of {e.MovementType} with Id = {e.Id}",
      };
      var json = JsonSerializer.Serialize(problemDetails);

      context.Response.StatusCode = (int)HttpStatusCode.Conflict;
      context.Response.ContentType = MediaTypeNames.Application.Json;
      await context.Response.WriteAsync(json);
    }
    catch (InsufficientProductLeftoverException e)
    {
      Log.Information(
        "Insufficient amount of product {ProductName}: Requested {RequestedQuantity}, left {Leftover}",
        e.Product.Name,
        e.RequestedQuantity,
        e.Leftover
      );

      var problemDetails = new ProblemDetails {
        Status = (int)HttpStatusCode.Conflict,
        Title = $"Insufficient amount of product {e.Product.Name}",
        Detail = $"Requested {e.RequestedQuantity}, left {e.Leftover}",
      };
      var json = JsonSerializer.Serialize(problemDetails);

      context.Response.StatusCode = (int)HttpStatusCode.Conflict;
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

      Log.Error(e.StackTrace!);

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