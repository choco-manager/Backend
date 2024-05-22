using Choco.Backend.Api.Domain.Products.Data;
using FastEndpoints;
using FluentValidation;

namespace Choco.Backend.Api.Domain.Products.Validation;

public class CreateProductRequestValidator: Validator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(e => e.Title).NotEmpty();
        RuleFor(e => e.RetailPrice).GreaterThan(0);
        RuleFor(e => e.CostPrice).GreaterThan(0);
    }
}