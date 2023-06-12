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

using Backend.Modules.Clients.Utils;

using FluentAssertions;

using NUnit.Framework;

#endregion


namespace Backend.Tests.Unit.Utils.Clients;

[TestFixture]
public class ClientsUtilsTests {
  private ClientUtils _utils = null!;

  [SetUp]
  public void Setup() {
    _utils = new ClientUtils();
  }

  [Test(Description = "Method to compare two lists of addresses should produce one list with delta of both lists")]
  public async Task CompareAddresses__WhenListsWasNotChanged__ReturnsSameList() {
    // Arrange
    var list = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

    // Act
    var result = _utils.GetDeltaOfGuidLists(list, list);

    // Assert
    result.Should().BeEquivalentTo(list);
  }


  [Test(Description = "Method to compare two lists of addresses should produce one list with delta of both lists")]
  public async Task CompareAddresses__WhenFirstListHasItemsThatSecondHasnt__CorrectDelta() {
    // Arrange
    var firstList = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
    var secondList = new List<Guid>() { firstList[0], firstList[1] };

    // Act
    var result = _utils.GetDeltaOfGuidLists(firstList, secondList);

    // Assert
    result.Should().BeEquivalentTo(secondList);
  }

  [Test(Description = "Method to compare two lists of addresses should produce one list with delta of both lists")]
  public async Task CompareAddresses__WhenSecondListHasItemsThatFirstHasnt__ReturnsCorrectDelta() {
    // Arrange
    var firstList = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
    var secondList = new List<Guid>() { firstList[0], firstList[1], firstList[2], Guid.NewGuid() };

    // Act
    var result = _utils.GetDeltaOfGuidLists(firstList, secondList);

    // Assert
    result.Should().BeEquivalentTo(secondList);
  }

  [Test(Description = "Method to compare two lists of addresses should produce one list with delta of both lists")]
  public async Task CompareAddresses__WhenBothListsHasItemsThatOtherHasnt__ReturnsCorrectDelta() {
    // Arrange
    var firstList = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
    var secondList = new List<Guid>() { firstList[0], firstList[1], Guid.NewGuid() };

    // Act
    var result = _utils.GetDeltaOfGuidLists(firstList, secondList);

    // Assert
    result.Should().BeEquivalentTo(secondList);
  }
}