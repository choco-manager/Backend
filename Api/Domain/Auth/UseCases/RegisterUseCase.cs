using Api.Common;
using Api.Data;
using Api.Domain.Auth.Data;
using Api.Extensions;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Auth.UseCases;

public class RegisterUseCase(AppDbContext db) : IUseCase<RegisterRequest, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Execute(RegisterRequest req, CancellationToken ct)
    {
        var ctx = ValidationContext<RegisterRequest>.Instance;

        if (ctx.ValidationFailed)
        {
            return Result.Invalid(ctx.ValidationFailures.AsErrors());
        }

        return Result.Success();
    }
}