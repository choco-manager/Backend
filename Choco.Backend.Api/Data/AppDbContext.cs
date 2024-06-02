using Choco.Backend.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public virtual DbSet<RevokedAccessToken> RevokedAccessTokens => Set<RevokedAccessToken>();
    public virtual DbSet<FcmToken> FcmTokens => Set<FcmToken>();
    public virtual DbSet<RestorationToken> RestorationTokens => Set<RestorationToken>();
    public virtual DbSet<Product> Products => Set<Product>();
    public virtual DbSet<ProductTag> ProductTags => Set<ProductTag>();
    public virtual DbSet<PriceHistory> PriceHistory => Set<PriceHistory>();
    public virtual DbSet<OrderedProduct> OrderedProducts => Set<OrderedProduct>();
    public virtual DbSet<Order> Orders => Set<Order>();
    public virtual DbSet<Address> Addresses => Set<Address>();
    public virtual DbSet<City> Cities => Set<City>();
    public virtual DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ProductTag>().HasData([
            new ProductTag
            {
                Id = Guid.Parse("ca398e3e-b307-4fb1-897d-0a879e07ed0a"),
                Title = "Молочный"
            },
            new ProductTag
            {
                Id = Guid.Parse("3f9c5572-2a30-4738-ad5d-e64a78d0b1c5"),
                Title = "Горький"
            },
            new ProductTag
            {
                Id = Guid.Parse("8983ea6f-d617-4b68-bf35-951e987b6df2"),
                Title = "Фирменный"
            },
            new ProductTag
            {
                Id = Guid.Parse("1cb55e22-b608-4eb1-9b00-bcf99227a45b"),
                Title = "Мармелад"
            },
            new ProductTag
            {
                Id = Guid.Parse("800ce8c6-cda8-4a33-8232-94d1039fdcbf"),
                Title = "На развес"
            },
            new ProductTag
            {
                Id = Guid.Parse("3916b26d-cb71-459d-9692-2c8376ed3222"),
                Title = "Брикет"
            },
            new ProductTag
            {
                Id = Guid.Parse("740c0d16-cff1-4fc2-9b32-198c41cd8f5b"),
                Title = "Конфеты"
            },
            new ProductTag
            {
                Id = Guid.Parse("66978e41-810b-4e15-b271-0185365319ea"),
                Title = "Паста"
            },
            new ProductTag
            {
                Id = Guid.Parse("6010b624-5e50-4c74-aedd-62d86968b8aa"),
                Title = "С орехами"
            },
        ]);

        modelBuilder.Entity<City>().HasData([
            new City
            {
                Id = Guid.Parse("2a872129-328e-4e39-9d06-16cb9bafe6fa"),
                Name = "Фурманов"
            },
            new City
            {
                Id = Guid.Parse("8ca6ebf2-bda0-4e56-9e2c-6dd77153433e"),
                Name = "Приволжск"
            },
            new City
            {
                Id = Guid.Parse("85ae3b9a-39d7-4654-a266-2c65000cf8f1"),
                Name = "Иваново"
            },
            new City
            {
                Id = Guid.Parse("06787c55-517b-4d26-9e54-5ae0755b65c2"),
                Name = "Владимир"
            },
        ]);

        modelBuilder.Entity<OrderedProduct>().HasKey(e => new { e.OrderId, e.ProductId });
    }
}