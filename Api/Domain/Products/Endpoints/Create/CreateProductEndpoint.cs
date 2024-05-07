using Api.Common;
using Api.Configuration.Swagger;
using Api.Domain.Products.Data;
using Api.Domain.Products.UseCases;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Products.Endpoints.Create;

public class CreateProductEndpoint(CreateProductUseCase createProductUseCase)
    : Endpoint<CreateProductRequest, Result<ProductDto>>
{
    public override void Configure()
    {
        Post("products");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Products));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<ProductDto>> ExecuteAsync(CreateProductRequest req, CancellationToken ct)
    {
        return await createProductUseCase.Execute(req, ct);
    }
}