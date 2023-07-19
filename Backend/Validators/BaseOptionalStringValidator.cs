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


using FluentValidation;


namespace Backend.Validators;

public class BaseOptionalStringValidator : AbstractValidator<string?> {
  public BaseOptionalStringValidator() {
    RuleFor(s => s)
      .NotEmpty().WithMessage("Строка не может быть пустой")
      .MinimumLength(3).WithMessage("Строка не может быть короче трёх символов")
      .Matches(@"^\p{L}+$")
      .WithMessage("Строка не может содержать специальных символов и цифр");
  }
}