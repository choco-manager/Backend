using Choco.Backend.Api.Common;
using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Products.Data;
using Choco.Backend.Api.Domain.Products.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Products.Endpoints.Update;

public class UpdateProductEndpoint(UpdateProductUseCase updateProductUseCase): Endpoint<UpdateProductRequest, Result<ProductDto>>
{
    public override async Task<Result<ProductDto>> ExecuteAsync(UpdateProductRequest req, CancellationToken ct)
    {
        return await updateProductUseCase.Execute(req, ct);
    }

    public override void Configure()
    {
        Patch("products/{id}");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Products));
        DontThrowIfValidationFails();
    }
}