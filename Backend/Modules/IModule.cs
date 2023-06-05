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

namespace Backend.Modules;

public interface IModule {
  IServiceCollection RegisterModule(IServiceCollection builder);
  IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}

public static class ModuleExtensions {
  private static readonly List<IModule> RegisteredModules = new();

  public static IServiceCollection RegisterModules(this IServiceCollection services) {
    var modules = DiscoverModules();
    foreach (var module in modules)
    {
      module.RegisterModule(services);
      RegisteredModules.Add(module);
    }

    return services;
  }

  public static WebApplication MapEndpoints(this WebApplication app) {
    foreach (var module in RegisteredModules)
    {
      module.MapEndpoints(app);
    }

    return app;
  }

  private static IEnumerable<IModule> DiscoverModules() {
    return typeof(IModule).Assembly
      .GetTypes()
      .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
      .Select(Activator.CreateInstance)
      .Cast<IModule>();
  }
}