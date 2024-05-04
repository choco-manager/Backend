using Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public virtual DbSet<RevokedAccessToken> RevokedAccessTokens => Set<RevokedAccessToken>();
    public virtual DbSet<FcmToken> FcmTokens => Set<FcmToken>();
}