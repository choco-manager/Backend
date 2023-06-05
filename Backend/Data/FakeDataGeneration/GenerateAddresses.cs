using Backend.Modules.Addresses.Contract;

using Bogus;

using Microsoft.EntityFrameworkCore;


namespace Backend.Data.FakeDataGeneration; 

public class GenerateAddresses {
  
  public GenerateAddresses() {
  }

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