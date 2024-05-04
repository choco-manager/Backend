using Ardalis.Result;
using FluentValidation;
using FluentValidation.Results;

namespace Api.Extensions;

public static class FluentValidationFailuresExtensions
{
    public static List<ValidationError> AsErrors(this List<ValidationFailure> failures)
    {
        List<ValidationError> validationErrorList = [];
        foreach (var error in failures)
            validationErrorList.Add(new ValidationError
            {
                Severity = FromSeverity(error.Severity),
                ErrorMessage = error.ErrorMessage,
                ErrorCode = error.ErrorCode,
                Identifier = error.PropertyName
            });
        return validationErrorList;
    }

    private static ValidationSeverity FromSeverity(Severity severity)
    {
        return severity switch
        {
            Severity.Error => ValidationSeverity.Error,
            Severity.Warning => ValidationSeverity.Warning,
            Severity.Info => ValidationSeverity.Info,
            _ => throw new ArgumentOutOfRangeException(nameof(severity), "Unexpected Severity")
        };
    }
}