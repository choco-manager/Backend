using Choco.Backend.Api.Domain.Orders.Data;
using FastEndpoints;
using FluentValidation;

namespace Choco.Backend.Api.Domain.Orders.Validation;

public class CreateOrderProductRequestValidator: Validator<CreateOrderProductRequest>
{
    public CreateOrderProductRequestValidator()
    {
        RuleFor(e => e.Amount).GreaterThan(0m);
    }
}