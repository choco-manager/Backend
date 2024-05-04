using System.Security.Claims;

namespace Api.Common;

public class AuthorizedEmptyRequest
{
    public required ClaimsPrincipal User { get; set; }
}