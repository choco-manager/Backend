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

using Backend.Modules.Cities.Contract;
using Backend.Modules.Clients.Contract;
using Backend.Modules.MovementStatuses.Contract;
using Backend.Modules.PriceChanges.Contract;
using Backend.Modules.ProductCategories.Contract;
using Backend.Modules.Products.Contract;

using Riok.Mapperly.Abstractions;

#endregion


namespace Backend.Data;

[Mapper]
public partial class Mappers {
  public partial City Map(CreateCityRequestBody city);
  public partial MovementStatus Map(CreateMovementStatusRequestBody movementStatus);
  public partial ProductCategory Map(CreateProductCategoryRequestBody productCategory);

  [MapProperty(nameof(Product.Category.Id), nameof(ProductDto.ProductCategoryId))]
  public partial ProductDto Map(Product product);

  public partial ClientDto Map(Client client);

  public ProductDetails Enhance(
    Product product,
    List<PriceChange> retailPrices,
    List<PriceChange> wholesalePrices
  ) {
    var dto = Enhance(product);

    dto.RetailPriceChanges = retailPrices;
    dto.WholesalePriceChanges = wholesalePrices;

    return dto;
  }

  private partial ProductDetails Enhance(Product product);
}