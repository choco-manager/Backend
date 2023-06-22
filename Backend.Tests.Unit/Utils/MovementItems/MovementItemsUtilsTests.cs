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
using Backend.Modules.MovementItems.Utils;
using Backend.Modules.Products.Contract;

using FluentAssertions;

using NUnit.Framework;

#endregion


namespace Backend.Tests.Unit.Utils.MovementItems;

[TestFixture]
public class MovementItemsUtilsTests {
  [Test]
  public void GetDifferencesFrom__ShouldReturnEmptyList__WhenNewListWasNotChanged() {
    // Arrange
    var oldList = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 1,
      },
    };

    // Act
    var result = oldList.GetDifferencesFrom(oldList);

    // Assert
    result.Should().BeEquivalentTo(new List<MovementItem>());
  }

  [Test]
  public void GetDifferencesFrom__ShouldGetThemCorrectly__WhenAddingNewProductToList() {
    // Arrange
    var oldList = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 1,
      },
    };
    var expectation = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 2",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 2,
      },
    };

    var newList = oldList.Concat(expectation);

    // Act
    var result = oldList.GetDifferencesFrom(newList);

    // Assert
    result.Should().BeEquivalentTo(expectation);
  }

  [Test]
  public void GetDifferencesFrom__ShouldGetThemCorrectly__WhenIncreasingAmountOfExistingItem() {
    // Arrange
    var oldList = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 1,
      },
    };
    var expectation = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 1,
      },
    };

    var newList = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 1,
      },
    };

    // Act
    var result = oldList.GetDifferencesFrom(newList);

    // Assert
    result.Should().BeEquivalentTo(expectation);
  }

  [Test]
  public void GetDifferencesFrom__ShouldGetThemCorrectly__WhenDecreasingAmountOfExistingItem() {
    // Arrange
    var oldList = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 2,
      },
    };
    var expectation = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = -1,
      },
    };

    var newList = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 1,
      },
    };

    // Act
    var result = oldList.GetDifferencesFrom(newList);

    // Assert
    result.Should().BeEquivalentTo(expectation);
  }

  [Test]
  public void GetDifferencesFrom__ShouldGetThemCorrectly__WhenRemovingExistingItem() {
    // Arrange
    var oldList = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 2,
      },
    };
    var expectation = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = -2,
      },
    };

    var newList = new List<MovementItem>();

    // Act
    var result = oldList.GetDifferencesFrom(newList);

    // Assert
    result.Should().BeEquivalentTo(expectation);
  }

  [Test]
  public void GetDifferencesFrom__ShouldGetThemCorrectly__WhenReplacingExistingItemWithNewOne() {
    // Arrange
    var oldList = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 2,
      },
    };
    var expectation = new List<MovementItem> {
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 1",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = -2,
      },
      new() {
        Product = new Product {
          Category = null,
          IsByWeight = false,
          IsDeleted = false,
          Name = "Товар 2",
          Id = Guid.Empty,
          RetailPrice = 300,
          WholesalePrice = 400,
          VkMarketId = 0,
        },
        Amount = 2,
      },
    };

    var newList = new List<MovementItem> {
      expectation[1],
    };

    // Act
    var result = oldList.GetDifferencesFrom(newList);

    // Assert
    result.Should().BeEquivalentTo(expectation);
  }
}