using System.Security.Claims;

namespace Choco.Backend.Api.Domain.Auth.Data;

public class AuthorizedLogoutRequest
{
    public required string AccessToken { get; set; }
    public ClaimsPrincipal User { get; set; }
}