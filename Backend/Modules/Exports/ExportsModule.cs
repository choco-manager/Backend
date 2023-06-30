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

using System.Net.Mime;

using Backend.Data;
using Backend.Data.Extensions;
using Backend.Modules.Exports.Utils;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion


namespace Backend.Modules.Exports;

public class ExportsModule : IModule {
  public IServiceCollection RegisterModule(IServiceCollection builder) {
    builder.AddSingleton<IExportUtils, ExportUtils>();
    return builder;
  }

  public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
    var module = endpoints.MapGroup("/api/export").WithTags("Export API");

    module.MapGet("image", ExportImage);

    return endpoints;
  }

  private async Task<IResult> ExportImage(
    [FromServices] ApplicationDbContext db,
    [FromServices] IExportUtils utils,
    [FromServices] Mappers mappers
  ) {
    var products = await db.Products
      .Where(p => !p.IsDeleted)
      .Include(p => p.Category)
      .OrderBy(p => p.Name)
      .Select(product => mappers.Cut(product, 0))
      .ToListAsync();

    foreach (var product in products)
    {
      product.Leftover = await db.MovementItems.GetLeftoverFor(product.Id);
    }

    var image = utils.GenerateImage(products.Where(p => p.Leftover > decimal.Zero).ToList()).ToArray();

    return TypedResults.File(image, MediaTypeNames.Image.Jpeg);
  }
}