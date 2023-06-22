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

using Backend.Modules.MovementStatuses.Contract;

#endregion


namespace Backend.Modules.MovementStatuses.Utils;

public static class MovementsStatusesUtils {
  public static bool IsChangePossible(this MovementStatus oldStatus, MovementStatus newStatus) {
    var oldStatusName = oldStatus.Name;
    var newStatusName = newStatus.Name;

    if (oldStatusName == newStatusName)
    {
      return true;
    }

    switch (oldStatusName)
    {
      case "Обрабатывается" when newStatusName is "Доставляется" or "Ожидает получения" or "Выполнен" or "Отменён":
      case "Доставляется" when newStatusName is "Ожидает получения" or "Выполнен":
      case "Ожидает получения" when newStatusName == "Выполнен":
        return true;
      default:
        return false;
    }
  }
}