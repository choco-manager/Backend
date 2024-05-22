using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.ProductTags.Data;
using Choco.Backend.Api.Domain.ProductTags.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.ProductTags.Endpoints.GetAll;

public class GetAllProductTagsEndpoint(GetAllProductTagsUseCase getAllProductTagsUseCase)
    : Endpoint<EmptyRequest, Result<ProductTagsResponse>>
{
    public override void Configure()
    {
        Get("product-tags");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.ProductTags));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<ProductTagsResponse>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return await getAllProductTagsUseCase.Execute(req, ct);
    }
}