using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Customers.Data;
using Choco.Backend.Api.Domain.Customers.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Customers.Endpoints.GetAll;

public class GetAllCustomersEndpoint(GetAllCustomersUseCase useCase): Endpoint<EmptyRequest, Result<ListOfCustomers>>
{
    public override void Configure()
    {
        Version(3);
        Get("/customers");
        Options(x => x.WithTags(SwaggerTags.Customers));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<ListOfCustomers>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return await useCase.Execute(req, ct);
    }
}