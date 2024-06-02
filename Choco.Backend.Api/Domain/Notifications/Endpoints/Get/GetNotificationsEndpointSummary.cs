using FastEndpoints;

namespace Choco.Backend.Api.Domain.Notifications.Endpoints.Get;

public class GetNotificationsEndpointSummary: Summary<GetNotificationsEndpoint>
{
    public GetNotificationsEndpointSummary()
    {
        Summary = "Gets all unread notifications and 10 read notifications; Marks unread notifications as read";
    }
}