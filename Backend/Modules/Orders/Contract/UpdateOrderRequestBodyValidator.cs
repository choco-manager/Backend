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

using FluentValidation;

#endregion


namespace Backend.Modules.Orders.Contract;

public class UpdateOrderRequestBodyValidator : AbstractValidator<UpdateOrderRequestBody> {
  public UpdateOrderRequestBodyValidator() {
    RuleFor(o => o.MovementStatusId).NotEmpty().NotEqual(Guid.Empty).NotNull();
    RuleFor(o => o.ClientId).NotEmpty().NotEqual(Guid.Empty).NotNull();
    RuleFor(o => o.SelectedAddressId).NotEmpty().NotEqual(Guid.Empty).NotNull();
    RuleForEach(o => o.Items).SetValidator(new UpdateMovementItemValidator());
  }
}