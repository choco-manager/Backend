using System.Data;
using Choco.Backend.Api.Domain.Auth.Data;
using FastEndpoints;
using FluentValidation;

namespace Choco.Backend.Api.Domain.Auth.Validation;

public class RegisterRequestValidator : Validator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(e => e.Login).NotEmpty().MinimumLength(3).MaximumLength(10);
        RuleFor(e => e.Password).NotEmpty().MinimumLength(6);
        RuleFor(e => e.Name).NotEmpty().MinimumLength(3).MaximumLength(10);
    }
}