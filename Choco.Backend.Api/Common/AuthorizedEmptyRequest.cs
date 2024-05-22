using System.Security.Claims;

namespace Choco.Backend.Api.Common;

public class AuthorizedEmptyRequest
{
    public required ClaimsPrincipal User { get; set; }
}