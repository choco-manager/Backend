using FastEndpoints;

namespace Api.Domain.Auth.Data;

public class LogoutRequest
{
    [FromHeader("Authorization")]
    public string BearerToken { get; set; }
}