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

#endregion


namespace Backend.Data.Pagination;

public class Paged<T> {
  public int PageNumber { get; init; }
  public int PageSize { get; init; }
  public int TotalCount { get; init; }
  public int TotalPages { get; init; }

  public List<T> Data { get; init; }


  public bool HasPreviousPage => PageNumber > 0;
  public bool HasNextPage => PageNumber + 1 < TotalPages;

  public static async Task<Paged<T>> Split(IQueryable<T> data, int pageNumber, int pageSize) {
    var totalCount = data.Count();
    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

    return new Paged<T> {
      PageNumber = pageNumber,
      PageSize = pageSize,
      TotalCount = totalCount,
      TotalPages = totalPages,

      Data = await data.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(),
    };
  }
}