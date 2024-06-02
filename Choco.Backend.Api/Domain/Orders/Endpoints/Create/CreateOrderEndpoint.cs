using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Orders.Data;
using Choco.Backend.Api.Domain.Orders.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.Create;

public class CreateOrderEndpoint(CreateOrderUseCase useCase): Endpoint<CreateOrderRequest, Result<IdModel>>
{
    public override void Configure()
    {
        Post("orders");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Orders));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<IdModel>> ExecuteAsync(CreateOrderRequest req, CancellationToken ct)
    {
        return await useCase.Execute(User, req, ct);
    }
}