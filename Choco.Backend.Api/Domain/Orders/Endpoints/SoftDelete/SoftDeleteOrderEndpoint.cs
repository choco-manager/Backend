using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Orders.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.SoftDelete;

public class SoftDeleteOrderEndpoint(SoftDeleteOrderUseCase useCase): Endpoint<IdModel, EmptyResponse>
{
    public override void Configure()
    {
        Delete("orders/{id}");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Orders));
        DontThrowIfValidationFails();
    }

    public override async Task<EmptyResponse> ExecuteAsync(IdModel req, CancellationToken ct)
    {
        return await useCase.Execute(req, ct);
    }
}