using Choco.Backend.Api.Domain.Orders.Data;
using FastEndpoints;
using FluentValidation;

namespace Choco.Backend.Api.Domain.Orders.Validation;

public class CreateOrderRequestValidator : Validator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(e => e.ToBeDeliveredAt).GreaterThanOrEqualTo(DateTime.UtcNow);
        RuleForEach(e => e.Products).SetValidator(new CreateOrderProductRequestValidator());
    }
}