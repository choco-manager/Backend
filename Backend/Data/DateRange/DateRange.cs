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

using FluentValidation;

#endregion


namespace Backend.Data.DateRange;

public class DateRange {
  public DateOnly StartDate { get; set; }
  public DateOnly EndDate { get; set; }

  public static bool TryParse(string? input, out DateRange? dateRange) {
    if (input is null)
    {
      dateRange = null;
      return true;
    }

    string[] dates;

    try
    {
      dates = input.Split(":");
    }
    catch (IndexOutOfRangeException)
    {
      dateRange = null;
      return false;
    }


    var startDateIsSucceeded = DateOnly.TryParse(dates[0], out var startDate);
    var endDateIsSucceeded = DateOnly.TryParse(dates[1], out var endDate);

    dateRange = new DateRange {
      StartDate = startDate,
      EndDate = endDate,
    };

    var validator = new DateRangeValidator();

    try
    {
      validator.ValidateAndThrow(dateRange);
    }
    catch (ValidationException e)
    {
      dateRange = null;
      return false;
    }

    return startDateIsSucceeded && endDateIsSucceeded;
  }
}