using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Data;

public class LoginRequest
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string FcmToken { get; set; }
}