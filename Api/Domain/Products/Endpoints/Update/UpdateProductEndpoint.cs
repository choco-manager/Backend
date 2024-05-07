using Api.Common;
using Api.Configuration.Swagger;
using Api.Domain.Products.Data;
using Api.Domain.Products.UseCases;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Products.Endpoints.Update;

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