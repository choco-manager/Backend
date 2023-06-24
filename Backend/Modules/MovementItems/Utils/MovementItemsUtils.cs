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

using Backend.Modules.MovementItems.Contract;
using Backend.Modules.Products.Utils;

#endregion


namespace Backend.Modules.MovementItems.Utils;

public static class MovementItemsUtils {
  public static List<MovementItem> GetDifferencesFrom(
    this IEnumerable<MovementItem> oldList,
    IEnumerable<MovementItem> newList
  ) {
    var difference = new List<MovementItem>();
    var oldItems = oldList as MovementItem[] ?? oldList.Optimized().ToArray();
    var newItems = newList as MovementItem[] ?? newList.ToArray();
    var comparer = new ProductEqualityComparer();


    foreach (var newItem in newItems)
    {
      // Searching for changed amounts
      var search = oldItems.FirstOrDefault(item => comparer.Equals(item.Product, newItem.Product));
      if (search is not null && search.Amount != newItem.Amount)
      {
        difference.Add(
          new MovementItem {
            Product = search.Product,
            Amount = newItem.Amount - search.Amount,
          }
        );
        continue;
      }

      // Adding new items
      if (oldItems.All(item => item != newItem))
      {
        difference.Add(newItem);
      }
    }

    // Adding deleted items
    difference.AddRange(oldItems
      .Where(oldItem => newItems.All(item => !comparer.Equals(item.Product, oldItem.Product)))
      .Select(oldItem => new MovementItem { Product = oldItem.Product, Amount = -oldItem.Amount })
    );

    return difference;
  }

  public static List<MovementItem> ApplyDifferences(
    this IEnumerable<MovementItem> oldList,
    IEnumerable<MovementItem> diff
  ) {
    var comparer = new ProductEqualityComparer();
    var oldItems = oldList.ToList();

    foreach (var diffItem in diff)
    {
      var search = oldItems.FirstOrDefault(item => comparer.Equals(diffItem.Product, item.Product));

      if (search is not null)
      {
        search.Amount += diffItem.Amount;
      }
      
    }

    oldItems.RemoveAll(item => item.Amount <= 0);

    return oldItems;
  }

  public static List<MovementItem> Optimized(this IEnumerable<MovementItem> list) {
    var result = new List<MovementItem>();
    var movementItems = list as MovementItem[] ?? list.ToArray();
    var comparer = new ProductEqualityComparer();

    foreach (var item in movementItems)
    {
      var movementItem = result.FirstOrDefault(i => comparer.Equals(i.Product, item.Product));

      if (movementItem is null)
      {
        result.Add(item);
      }
      else
      {
        movementItem.Amount += item.Amount;
      }
    }

    return result;
  }
}