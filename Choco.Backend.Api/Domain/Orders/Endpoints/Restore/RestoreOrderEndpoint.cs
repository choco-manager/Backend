using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Orders.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.Restore;

public class RestoreOrderEndpoint(RestoreOrderUseCase useCase) : Endpoint<IdModel, Result<EmptyResponse>>
{
    public override void Configure()
    {
        Put("orders/{id}");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Orders));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<EmptyResponse>> ExecuteAsync(IdModel req, CancellationToken ct)
    {
        return await useCase.Execute(req, ct);
    }
}