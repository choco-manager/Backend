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

using Microsoft.EntityFrameworkCore;

using MigrateData.Data.Models;

#endregion


namespace MigrateData.Data;

public class OldAppDbContext : DbContext {
  public DbSet<OldProduct> Products => Set<OldProduct>();
  public DbSet<OldProductCategory> ProductCategories => Set<OldProductCategory>();
  public DbSet<OldShipment> Shipments => Set<OldShipment>();
  public DbSet<OldShipmentStatus> ShipmentStatuses => Set<OldShipmentStatus>();
  public DbSet<OldShipmentItem> ShipmentItems => Set<OldShipmentItem>();
  public DbSet<OldOrder> Orders => Set<OldOrder>();
  public DbSet<OldOrderItem> OrderItems => Set<OldOrderItem>();
  public DbSet<OldOrderStatus> OrderStatuses => Set<OldOrderStatus>();
  public DbSet<OldOrderCity> OrderCities => Set<OldOrderCity>();
  public DbSet<OldOrderAddress> OrderAddresses => Set<OldOrderAddress>();

  public OldAppDbContext(DbContextOptions<OldAppDbContext> contextOptions) : base(contextOptions) { }
}