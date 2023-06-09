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


namespace Backend.Modules.Products.Contract;

public class UpdateProductRequestBody {
  public string Name { get; set; }
  public Guid CategoryId { get; set; }
  public int WholesalePrice { get; set; }
  public int RetailPrice { get; set; }
  public bool IsByWeight { get; set; }
  public int VkMarketId { get; set; }
}