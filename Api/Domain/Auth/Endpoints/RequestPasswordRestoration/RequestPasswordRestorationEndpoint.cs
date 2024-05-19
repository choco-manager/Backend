﻿using Api.Configuration.Swagger;
using Api.Domain.Auth.Data;
using Api.Domain.Auth.UseCases;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Auth.Endpoints.RequestPasswordRestoration;

public class RequestPasswordRestorationEndpoint(RequestPasswordRestorationUseCase requestPasswordRestorationUseCase)
    : Endpoint<UsernameRequest, Result<RestorationTokenResponse>>
{
    public override void Configure()
    {
        Post("auth/request-restoration");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Auth));
        DontThrowIfValidationFails();
        AllowAnonymous();
    }

    public override async Task<Result<RestorationTokenResponse>> ExecuteAsync(UsernameRequest req, CancellationToken ct)
    {
        return await requestPasswordRestorationUseCase.Execute(req, ct);
    }
}