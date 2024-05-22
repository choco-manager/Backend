using Choco.Backend.Api.Domain.Auth.Data;
using FastEndpoints;
using FluentValidation;

namespace Choco.Backend.Api.Domain.Auth.Validation;

public class RestorePasswordRequestValidator : Validator<RestorePasswordRequest>
{
    public RestorePasswordRequestValidator()
    {
        RuleFor(e => e.NewPassword).NotEmpty().MinimumLength(6);
    }
}