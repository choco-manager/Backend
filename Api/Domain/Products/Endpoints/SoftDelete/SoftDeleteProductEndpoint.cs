using Api.Common;
using Api.Configuration.Swagger;
using Api.Domain.Products.Data;
using Api.Domain.Products.UseCases;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Products.Endpoints.SoftDelete;

public class SoftDeleteProductEndpoint(SoftDeleteProductUseCase softDeleteProductUseCase)
    : Endpoint<IdModel, Result<ProductDto>>
{
    public override void Configure()
    {
        Delete("products/{id:guid}");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Products));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<ProductDto>> ExecuteAsync(IdModel req, CancellationToken ct)
    {
        return await softDeleteProductUseCase.Execute(req, ct);
    }
}