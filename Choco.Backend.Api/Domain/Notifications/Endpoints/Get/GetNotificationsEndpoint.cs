using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Notifications.Data;
using Choco.Backend.Api.Domain.Notifications.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Notifications.Endpoints.Get;

public class GetNotificationsEndpoint(GetNotificationsUseCase useCase)
    : Endpoint<EmptyRequest, Result<NotificationsList>>
{
    public override void Configure()
    {
        Version(3);
        Get("/notifications");
        Options(x => x.WithTags(SwaggerTags.Notifications));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<NotificationsList>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return await useCase.Execute(User, req, ct);
    }
}