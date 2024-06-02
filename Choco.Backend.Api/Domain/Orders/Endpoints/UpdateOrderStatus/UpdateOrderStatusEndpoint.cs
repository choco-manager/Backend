using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Orders.Data;
using Choco.Backend.Api.Domain.Orders.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.UpdateOrderStatus;

public class UpdateOrderStatusEndpoint(UpdateOrderStatusUseCase useCase): Endpoint<UpdateOrderStatusRequest, Result<IdModel>>
{
    public override void Configure()
    {
        Version(3);
        Patch("/orders/{id}/status");
        Options(x => x.WithTags(SwaggerTags.Orders));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<IdModel>> ExecuteAsync(UpdateOrderStatusRequest req, CancellationToken ct)
    {
        return await useCase.Execute(req, ct);
    }
}