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
using Backend.Modules.Cities.Contract;
using Backend.Modules.Clients.Contract;
using Backend.Modules.MovementItems.Contract;
using Backend.Modules.MovementStatuses.Contract;
using Backend.Modules.Orders.Contract;
using Backend.Modules.PriceChanges.Contract;
using Backend.Modules.ProductCategories.Contract;
using Backend.Modules.Products.Contract;
using Backend.Modules.Shipments.Contract;

using Riok.Mapperly.Abstractions;

#endregion


namespace Backend.Data;

[Mapper]
public partial class Mappers {
  public partial City Enhance(CreateCityRequestBody city);
  public partial MovementStatus Enhance(CreateMovementStatusRequestBody movementStatus);
  public partial ProductCategory Enhance(CreateProductCategoryRequestBody productCategory);

  public ProductDetails Enhance(
    Product product,
    List<PriceChange> retailPrices,
    List<PriceChange> wholesalePrices,
    decimal leftover
  ) {
    var dto = Enhance(product);

    dto.RetailPriceChanges = retailPrices;
    dto.WholesalePriceChanges = wholesalePrices;
    dto.Leftover = leftover;

    return dto;
  }

  private partial ProductDetails Enhance(Product product);

  public ProductDto Cut(Product product, decimal leftover) {
    var dto = Cut(product);

    dto.Leftover = leftover;

    return dto;
  }

  [MapProperty(new[] { nameof(Product.Category), nameof(Product.Category.Name) },
    new[] { nameof(ProductDto.CategoryName) })]
  private partial ProductDto Cut(Product product);

  public partial ClientDto Cut(Client client);

  [MapProperty(new[] { nameof(Order.Status), nameof(Order.Status.Name) }, new[] { nameof(OrderDto.StatusName) })]
  public partial OrderDto Cut(Order order);

  public partial ShipmentDto Cut(Shipment shipment);

  public partial UpdateClientRequestBody CutToRb(Client client);

  private Guid AddressToGuid(Address address) {
    return address.Id;
  }

  private string AddressToReadableAddress(Address address) {
    return $"г. {address.City.Name}, {address.Street}, {address.Building}";
  }

  private string MovementItemToString(MovementItem movementItem) {
    var unit = movementItem.Product.IsByWeight ? "шт" : "кг";
    return $"{movementItem.Product.Name} x{movementItem.Amount} {unit}.";
  }
}