using Api.Domain.Auth.Data;
using FastEndpoints;
using FluentValidation;

namespace Api.Domain.Auth.Validation;

public class LoginRequestValidator: Validator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(e => e.Login).NotEmpty().MinimumLength(3).MaximumLength(10);
        RuleFor(e => e.Password).NotEmpty().MinimumLength(6);
    }
}