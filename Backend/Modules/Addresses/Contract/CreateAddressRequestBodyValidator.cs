using FluentValidation;


namespace Backend.Modules.Addresses.Contract;

public class CreateAddressRequestBodyValidator : AbstractValidator<CreateAddressRequestBody> {
  public CreateAddressRequestBodyValidator() {
    RuleFor(address => address.City).NotEmpty().NotEqual(Guid.Empty).NotNull();
    RuleFor(address => address.Street).NotEmpty().NotNull();
    RuleFor(address => address.Building).NotEmpty().NotNull();
  }
}