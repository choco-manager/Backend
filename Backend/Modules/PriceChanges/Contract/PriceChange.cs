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

using System.Text.Json.Serialization;

using Backend.Data;
using Backend.Modules.PriceTypes.Contract;
using Backend.Modules.Products.Contract;

#endregion


namespace Backend.Modules.PriceChanges.Contract;

public class PriceChange : BaseModel {
  [JsonIgnore]
  public override Guid Id { get; set; }

  [JsonIgnore]
  public Product Product { get; set; }

  [JsonIgnore]
  public PriceType PriceType { get; set; }
  public DateTime ChangeTimestamp { get; set; }
  public int Price { get; set; }
}