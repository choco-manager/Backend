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

using Backend.Data;
using Backend.Modules.MovementStatuses.Contract;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.MovementStatuses;

public class MovementStatusesModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder
      .AddSingleton<AbstractValidator<CreateMovementStatusRequestBody>, CreateMovementStatusRequestBodyValidator>();

    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    var module = endpoints
      .MapGroup("/api/movementStatuses")
      .WithTags("Movement Statuses API");

    module.MapGet("", GetMovementStatuses);
    module.MapPost("", CreateMovementStatusHandler);

    return endpoints;
  }

  [SwaggerOperation(Summary = "Gets all available movement statuses")]
  [SwaggerResponse(200, "Movement statuses was returned successfully", typeof(List<MovementStatus>))]
  [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
  private static async Task<IResult> GetMovementStatuses([FromServices] ApplicationDbContext db) {
    using var op = Operation.Begin("Requesting movement statuses");
    var movementStatuses = await db.MovementStatuses.ToListAsync();
    op.Complete();
    Log.Information("Fetched {Count} movement statuses", movementStatuses.Count);
    return TypedResults.Ok(movementStatuses);
  }

  [SwaggerOperation(Summary = "Creates new movement status")]
  [SwaggerResponse(201, "Movement status was created successfully", typeof(MovementStatus))]
  [SwaggerResponse(400, "Invalid data was passed", typeof(ProblemDetails))]
  [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
  private static async Task<IResult> CreateMovementStatusHandler(
    [FromBody] [SwaggerRequestBody(Description = "Name of the movement status to create", Required = true)]
    CreateMovementStatusRequestBody movementStatus,
    [FromServices] ApplicationDbContext db,
    [FromServices] AbstractValidator<CreateMovementStatusRequestBody> validator,
    [FromServices] Mappers mapper
  ) {
    await validator.ValidateAndThrowAsync(movementStatus);

    using var op = Operation.Begin("Saving new movement status");
    var entity = mapper.Enhance(movementStatus);
    await db.MovementStatuses.AddAsync(entity);
    await db.SaveChangesAsync();
    op.Complete("Movement status", movementStatus.Name);

    Log.Information("Added new movement status '{Name}'", movementStatus.Name);
    return TypedResults.Created("/api/movementStatuses", entity);
  }
}