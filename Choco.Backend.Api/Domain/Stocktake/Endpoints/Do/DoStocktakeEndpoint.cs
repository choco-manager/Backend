using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Stocktake.Data;
using Choco.Backend.Api.Domain.Stocktake.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Stocktake.Endpoints.Do;

public class DoStocktakeEndpoint(DoStocktakeUseCase doStocktakeUseCase): Endpoint<DoStocktakeRequest, Result<DoStocktakeResponse>>
{
    public override void Configure()
    {
        Post("stocktaking/{id}");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Stocktaking));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<DoStocktakeResponse>> ExecuteAsync(DoStocktakeRequest req, CancellationToken ct)
    {
        return await doStocktakeUseCase.Execute(req, ct);
    }
}