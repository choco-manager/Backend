using Api.Common;
using Api.Configuration.Swagger;
using Api.Domain.Products.Data;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Products.Endpoints.GetAll;

public class GetAllProductsEndpoint(IPagedUseCase<PagedRequest, List<ProductDto>> getAllProductsUseCase): Endpoint<PagedRequest, PagedResult<List<ProductDto>>>
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