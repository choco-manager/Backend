﻿using FastEndpoints;

namespace Api.Domain.Auth.Endpoints.RequestPasswordRestoration;

public class RequestPasswordRestorationEndpointSummary : Summary<RequestPasswordRestorationEndpoint>
{
    public RequestPasswordRestorationEndpointSummary()
    {
        Summary = "Requests a token to restore user's password";
    }
}