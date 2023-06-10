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
using Backend.Modules.Clients.Contract;

using Bogus;

using Microsoft.EntityFrameworkCore;

#endregion


namespace Backend.Data.FakeDataGeneration;

public class GenerateClients {
  public GenerateClients() { }

  public async Task<List<Client>> Generate(ApplicationDbContext context) {
    var addresses = await context.Addresses.ToListAsync();

    var clients = new Faker<Client>("ru")
      .StrictMode(true)
      .RuleFor(c => c.Id, f => f.Random.Guid())
      .RuleFor(c => c.FirstName, f => f.Name.FirstName())
      .RuleFor(c => c.LastName, f => f.Name.LastName())
      .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
      .RuleFor(c => c.ChatLink, f => $"https://vk.com/gim196048424?sel={f.Random.Int(100000000)}")
      .RuleFor(c => c.Addresses, f => new List<Address>(f.PickRandom(addresses, f.Random.Int(1, 3))));

    var generated = clients.GenerateBetween(10, 20);
    await context.Clients.AddRangeAsync(generated);
    await context.SaveChangesAsync();

    return generated;
  }
}