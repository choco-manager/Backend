using FluentValidation;

namespace Backend.Modules.Cities.Contract;

public class CityValidator: AbstractValidator<City>
{
    public CityValidator()
    {
        RuleFor(city => city.Name).NotEmpty().NotNull();
    }
}