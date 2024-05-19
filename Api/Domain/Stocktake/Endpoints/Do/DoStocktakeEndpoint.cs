using Api.Configuration.Swagger;
using Api.Domain.Stocktake.Data;
using Api.Domain.Stocktake.UseCases;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Stocktake.Endpoints.Do;

public class DoStocktakeEndpoint(DoStocktakeUseCase doStocktakeUseCase): Endpoint<DoStocktakeRequest, Result<DoStocktakeResponse>>
{
    public override void Configure()
    {
        Post("stocktaking");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Stocktaking));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<DoStocktakeResponse>> ExecuteAsync(DoStocktakeRequest req, CancellationToken ct)
    {
        return await doStocktakeUseCase.Execute(req, ct);
    }
}