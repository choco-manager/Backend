using Choco.Backend.Api.Common;
using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Products.Data;
using Choco.Backend.Api.Domain.Products.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Products.Endpoints.Create;

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