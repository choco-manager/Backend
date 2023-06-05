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

using Backend.Modules.Addresses.Contract;

using Bogus;

using Microsoft.EntityFrameworkCore;

#endregion


namespace Backend.Data.FakeDataGeneration;

public class GenerateAddresses {
  public GenerateAddresses() { }

  public async Task<List<Address>> Generate(ApplicationDbContext context) {
    var cities = await context.Cities.ToListAsync();

    var addresses = new Faker<Address>("ru")
      .StrictMode(true)
      .RuleFor(a => a.Id, f => Guid.NewGuid())
      .RuleFor(a => a.City, f => f.PickRandom(cities))
      .RuleFor(a => a.Street, f => f.Address.StreetName())
      .RuleFor(a => a.Building, f => f.Address.BuildingNumber());

    var generated = addresses.GenerateBetween(10, 20);
    await context.Addresses.AddRangeAsync(generated);
    await context.SaveChangesAsync();

    return generated;
  }
}