using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Orders.Data;
using Choco.Backend.Api.Domain.Orders.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.GetAll;

public class GetAllOrdersEndpoint(GetAllOrdersUseCase useCase): Endpoint<PagedRequest, PagedResult<ListOfOrders>>
{
    public override void Configure()
    {
        Get("orders");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Orders));
        DontThrowIfValidationFails();
    }

    public override async Task<PagedResult<ListOfOrders>> ExecuteAsync(PagedRequest req, CancellationToken ct)
    {
        return await useCase.Execute(req, ct);
    }
}