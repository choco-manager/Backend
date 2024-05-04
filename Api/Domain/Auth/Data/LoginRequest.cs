using FastEndpoints;

namespace Api.Domain.Auth.Data;

public class LoginRequest
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    [FromHeader("Choco-FcmToken")]
    public required string FcmToken { get; set; }
}