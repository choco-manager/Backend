using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Customers.Data;
using Choco.Backend.Api.Domain.Customers.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Customers.Endpoints.Create;

public class CreateCustomerEndpoint(CreateCustomerUseCase useCase): Endpoint<CreateCustomerRequest, Result<IdModel>>
{
    public override void Configure()
    {
        Version(3);
        Post("/customers");
        Options(x => x.WithTags(SwaggerTags.Customers));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<IdModel>> ExecuteAsync(CreateCustomerRequest req, CancellationToken ct)
    {
        return await useCase.Execute(req, ct);
    }
}