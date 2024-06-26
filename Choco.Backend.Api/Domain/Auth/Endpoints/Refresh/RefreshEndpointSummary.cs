﻿using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.Refresh;

public class RefreshEndpointSummary: Summary<RefreshEndpoint>
{
    public RefreshEndpointSummary()
    {
        Summary = "Refreshes access token";
    }
}