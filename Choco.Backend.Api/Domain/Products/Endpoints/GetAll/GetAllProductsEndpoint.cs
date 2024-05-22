using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Products.Data;
using Choco.Backend.Api.Domain.Products.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Products.Endpoints.GetAll;

public class GetAllProductsEndpoint(GetAllProductsUseCase getAllProductsUseCase): Endpoint<PagedRequest, PagedResult<List<ProductDto>>>
{
    public override void Configure()
    {
        Get("products");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Products));
        DontThrowIfValidationFails();
    }

    public override async Task<PagedResult<List<ProductDto>>> ExecuteAsync(PagedRequest req, CancellationToken ct)
    {
        return await getAllProductsUseCase.Execute(req, ct);
    }
}