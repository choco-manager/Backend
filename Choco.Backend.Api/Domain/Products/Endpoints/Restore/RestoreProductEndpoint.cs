using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Products.Data;
using Choco.Backend.Api.Domain.Products.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Products.Endpoints.Restore;

public class RestoreProductEndpoint(RestoreProductUseCase restoreProductUseCase) : Endpoint<IdModel, Result<ProductDto>>
{
    public override void Configure()
    {
        Put("products/{id:guid}");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Products));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<ProductDto>> ExecuteAsync(IdModel req, CancellationToken ct)
    {
        return await restoreProductUseCase.Execute(req, ct);
    }
}