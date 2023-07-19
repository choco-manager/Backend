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


using Backend.Attributes;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;


namespace Backend.SwaggerConfiguration.Filters;

public class HideFromSwaggerFilter : IDocumentFilter {
  public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) {
    foreach (var apiDescription in context.ApiDescriptions)
    {
      var succeeded = apiDescription.TryGetMethodInfo(out var methodInfo);
      if (!succeeded || !methodInfo.GetCustomAttributes(typeof(HideFromSwaggerAttribute), false).Any())
      {
        continue;
      }

      var route = "/" + apiDescription.RelativePath?.TrimEnd('/');
      swaggerDoc.Paths.Remove(route!);
    }
  }
}