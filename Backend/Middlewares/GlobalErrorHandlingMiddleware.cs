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
        Title = "Ошибка валидации",
        Detail = string.Join(", ",
          e.Errors.Select(failure =>
            $"Ошибка в свойстве {failure.PropertyName}: {failure.ErrorMessage} (передано значение '{failure.AttemptedValue}')")),
      };

      var json = JsonSerializer.Serialize(problemDetails);
      context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      context.Response.ContentType = MediaTypeNames.Application.Json;
      await context.Response.WriteAsync(json);
    }
    catch (EntityWasNotFoundException e)
    {
      var title = $"{e.Name} не найдена";
      string detail;

      if (e.Id is not null)
      {
        Log.Information("{Entity} не найдена", e.Name);
        detail = $"{e.Name} с идентификатором {e.Id} не существует в базе данных";
      }
      else
      {
        Log.Information("{Entity} не найдена", e.Name);
        detail = $"{e.Name} по запросу {e.Query} не существует в базе данных";
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
        Title = $"Нельзя сменить статус {e.OldStatus.Name} -> {e.NewStatus.Name}",
        Detail = $"Не получилось изменить статус {e.MovementType} с идентификатором {e.Id}",
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
        Title = $"Недостаточно товара {e.Product.Name}",
        Detail = $"Запрошено {e.RequestedQuantity}, осталось {e.Leftover}",
      };
      var json = JsonSerializer.Serialize(problemDetails);

      context.Response.StatusCode = (int)HttpStatusCode.Conflict;
      context.Response.ContentType = MediaTypeNames.Application.Json;
      await context.Response.WriteAsync(json);
    }
    catch (InvalidFormatOfDateRangeStringException e)
    {
      Log.Information("Unable to parse date range: {DateRangeString}", e.InvalidString);

      var problemDetails = new ProblemDetails {
        Status = (int)HttpStatusCode.BadRequest,
        Title = $"Ошибка разбора диапазона дат {e.InvalidString}",
        Detail = $"Требуемый формат ГГГГ-ММ-ДД:ГГГГ-ММ-ММ",
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