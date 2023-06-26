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
using Backend.Modules.PriceTypes.Contract;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

using SerilogTimings;

using Swashbuckle.AspNetCore.Annotations;

#endregion


namespace Backend.Modules.PriceTypes;

public class PriceTypesModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/api/priceTypes", GetAllPriceTypesHandler).WithTags("Price Types API");

    return endpoints;
  }

  [SwaggerOperation(Summary = "Gets all available price types")]
  [SwaggerResponse(200, "Price types was returned successfully", typeof(List<PriceType>))]
  [SwaggerResponse(500, "Unexpected error", typeof(ProblemDetails))]
  private async Task<IResult> GetAllPriceTypesHandler([FromServices] ApplicationDbContext db) {
    using var op = Operation.Begin("Requesting price types");
    var priceTypes = await db.PriceTypes.ToListAsync();
    op.Complete();
    Log.Information("Fetched {Count} price types", priceTypes.Count);
    return TypedResults.Ok(priceTypes);
  }
}