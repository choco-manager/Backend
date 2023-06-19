#region

using Backend.Data;
using Backend.Exceptions;
using Backend.Modules.Addresses.Contract;
using Backend.Modules.Cities.Contract;
using Backend.Modules.PriceChanges.Contract;
using Backend.Modules.PriceTypes.Contract;
using Backend.Modules.Products.Contract;

using Microsoft.EntityFrameworkCore;

using MigrateData;
using MigrateData.Data;

#endregion


var builder = WebApplication.CreateBuilder(args);

var oldConnectionString = builder.Configuration.GetConnectionString("Old");
var newConnectionString = builder.Configuration.GetConnectionString("New");

builder.Services.AddDbContext<OldAppDbContext>(options => options.UseNpgsql(oldConnectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(newConnectionString));

var app = builder.Build();

app.MapGet("/", async (OldAppDbContext oldDb, ApplicationDbContext newDb) => {
  // Get old and new cities to compare them
  var oldCities = await oldDb.OrderCities.ToListAsync();
  var newCities = await newDb.Cities.ToListAsync();

  // Fill mapper with id of old city and id of new city
  var citiesMap = new Dictionary<Guid, Guid>();
  foreach (var city in oldCities)
  {
    var newCity = newCities.FirstOrDefault(c => c.Name == city.Name);
    if (newCity is not null)
    {
      citiesMap.Add(city.Id, newCity.Id);
    }
  }

  var newAddresses = new List<Address>();

  var oal = await oldDb.OrderAddresses.ToListAsync();

  var oldAddresses = oal
    .GroupBy(a => new { a.City, a.Street, a.Building })
    .Select(g => g.First());

  foreach (var oldOrderAddress in oldAddresses)
  {
    var cityId = citiesMap[oldOrderAddress.City.Id];
    newAddresses.Add(new Address {
      City = await newDb.Cities.FindAsync(cityId) ?? throw new EntityWasNotFoundException(nameof(City), cityId),
      Street = oldOrderAddress.Street,
      Building = oldOrderAddress.Building,
    });
  }

  await newDb.Addresses.AddRangeAsync(newAddresses);

  // Get old and new categories to compare them
  var oldCategories = await oldDb.ProductCategories.ToListAsync();
  var newCategories = await newDb.ProductCategories.ToListAsync();

  // Fill mapper with id of old category and id of new category
  var categoriesMap = new Dictionary<Guid, Guid>();
  foreach (var category in oldCategories)
  {
    var newCategory = newCategories.FirstOrDefault(c => c.Name == category.Name);
    if (newCategory is not null)
    {
      categoriesMap.Add(category.Id, newCategory.Id);
    }
  }

  var newProducts = new List<Product>();
  var priceHistory = new List<PriceChange>();

  foreach (var product in oldDb.Products)
  {
    var categoryId = categoriesMap[product.Category.Id];
    var newProduct = new Product {
      Category = await newDb.ProductCategories.FindAsync(categoryId) ??
        throw new EntityWasNotFoundException(nameof(City), categoryId),
      Name = product.Name,
      IsByWeight = product.IsByWeight,
      IsDeleted = product.Deleted,
      VkMarketId = product.MarketId,
      WholesalePrice = product.WholesalePrice,
      RetailPrice = product.RetailPrice,
    };
    newProducts.Add(newProduct);
    priceHistory.Add(new PriceChange {
      ChangeTimestamp = DateTime.Now,
      Price = product.WholesalePrice,
      Product = newProduct,
      PriceType = await newDb.PriceTypes.FindAsync(PriceTypeEnum.Wholesale) ??
        throw new EntityWasNotFoundException(nameof(PriceType), PriceTypeEnum.Wholesale),
    });
    priceHistory.Add(new PriceChange {
      ChangeTimestamp = DateTime.Now,
      Price = product.RetailPrice,
      Product = newProduct,
      PriceType = await newDb.PriceTypes.FindAsync(PriceTypeEnum.Retail) ??
        throw new EntityWasNotFoundException(nameof(PriceType), PriceTypeEnum.Retail),
    });
  }

  await newDb.Products.AddRangeAsync(newProducts);
  await newDb.PriceChanges.AddRangeAsync(priceHistory);


  return TypedResults.Ok(new Result {
    Addresses = newAddresses,
    PriceChanges = priceHistory,
    Products = newProducts,
  });
});

app.Run();