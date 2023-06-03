using Backend.Modules.Addresses.Contract;
using Backend.Modules.Cities.Contract;
using Backend.Modules.Clients.Contract;
using Backend.Modules.MovementItems.Contract;
using Backend.Modules.MovementStatuses.Contract;
using Backend.Modules.Orders.Contract;
using Backend.Modules.PriceChanges.Contract;
using Backend.Modules.PriceTypes.Contract;
using Backend.Modules.ProductCategories.Contract;
using Backend.Modules.Products.Contract;
using Backend.Modules.Shipments.Contract;

using Microsoft.EntityFrameworkCore;


namespace Backend.Data;

public class ApplicationDbContext : DbContext {
  public DbSet<Address> Addresses => Set<Address>();
  public DbSet<City> Cities => Set<City>();
  public DbSet<Client> Clients => Set<Client>();
  public DbSet<MovementItem> MovementItems => Set<MovementItem>();
  public DbSet<MovementStatus> MovementStatuses => Set<MovementStatus>();
  public DbSet<Order> Orders => Set<Order>();
  public DbSet<PriceChange> PriceChanges => Set<PriceChange>();
  public DbSet<PriceType> PriceTypes => Set<PriceType>();
  public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
  public DbSet<Product> Products => Set<Product>();
  public DbSet<Shipment> Shipments => Set<Shipment>();

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<City>().HasData(
      new City { Id = new Guid("ca5e9b9c-05c3-4e95-83c2-eca36aad8500"), Name = "Фурманов" },
      new City { Id = new Guid("219a2ce6-d598-4067-925c-03eb345b1a76"), Name = "Владимир" },
      new City { Id = new Guid("49c95aa3-c980-4352-9230-683f28a727ed"), Name = "Приволжск" },
      new City { Id = new Guid("ba1d50cb-74a8-4ae0-88e1-01c8a6e87618"), Name = "Иваново" }
    );

    modelBuilder.Entity<MovementStatus>().HasData(
      new MovementStatus { Id = new Guid("8bec9a5a-ed58-487b-a488-a641e3c601cb"), Name = "Обрабатывается" },
      new MovementStatus { Id = new Guid("c1c5868a-b7fc-4c58-a8a3-b73b47e2fb9e"), Name = "Доставляется" },
      new MovementStatus { Id = new Guid("59f8dbbf-c17c-4f25-9e70-8760210422df"), Name = "Ожидает получения" },
      new MovementStatus { Id = new Guid("0ba48b38-b268-41a5-b228-f8a93590f5d5"), Name = "Выполнен" },
      new MovementStatus { Id = new Guid("d50d7272-05ca-4e4f-9769-475a7984be77"), Name = "Отменён" }
    );

    modelBuilder.Entity<PriceType>().HasData(
      new PriceType { Id = new Guid("3165c0b3-f92f-4a2d-8278-d131b8a32478"), Name = "Опт" },
      new PriceType { Id = new Guid("0d501699-abaf-42ad-a858-eaacc5c50199"), Name = "Розница" }
    );

    modelBuilder.Entity<ProductCategory>().HasData(
      new ProductCategory { Id = new Guid("57902a45-c181-4450-ad49-cdaed2faef01"), Name = "Молочные" },
      new ProductCategory { Id = new Guid("9b52555d-9bb2-4913-9ca8-b8503d21304b"), Name = "Горькие" },
      new ProductCategory { Id = new Guid("96e0ebf5-b331-4ec1-8993-ce84c0306a0a"), Name = "Белые" },
      new ProductCategory { Id = new Guid("7b5afc77-f10a-4ea9-bd03-b7ae67e83ea2"), Name = "Фирменные" },
      new ProductCategory { Id = new Guid("7eb7cdcd-6bb7-46c1-ae35-3b78d51ea2b3"), Name = "Мармелады" },
      new ProductCategory { Id = new Guid("20cdd5da-445d-4361-83b1-19cfef0542ba"), Name = "Конфеты" },
      new ProductCategory { Id = new Guid("40c2b950-5e3f-45aa-8e7a-20d6693b250c"), Name = "Пасты" }
    );
    base.OnModelCreating(modelBuilder);
  }
}