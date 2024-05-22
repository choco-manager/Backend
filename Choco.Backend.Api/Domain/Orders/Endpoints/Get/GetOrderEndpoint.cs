using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Orders.Data;
using Choco.Backend.Api.Domain.Orders.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.Get;

public class GetOrderEndpoint(GetOrderUseCase useCase): Endpoint<IdModel, Result<OrderDto>>
{
    public override void Configure()
    {
        Get("orders/{id}");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Orders));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<OrderDto>> ExecuteAsync(IdModel req, CancellationToken ct)
    {
        return await useCase.Execute(req, ct);
    }
}