using Api.Domain.Auth.Data;
using FastEndpoints;
using FluentValidation;

namespace Api.Domain.Auth.Validation;

public class RegisterRequestValidator: Validator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(e => e.Login).NotEmpty().MinimumLength(3);
        RuleFor(e => e.Password).NotEmpty().MinimumLength(6);
    }
}