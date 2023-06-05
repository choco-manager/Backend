using FluentValidation;


namespace Backend.Modules.Cities.Contract;

public class CityRequestBodyValidator : AbstractValidator<CreateCityRequestBody> {
  public CityRequestBodyValidator() {
    RuleFor(city => city.Name).NotEmpty().NotNull();
  }
}