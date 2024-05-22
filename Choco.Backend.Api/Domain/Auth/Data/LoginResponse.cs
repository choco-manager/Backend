namespace Choco.Backend.Api.Domain.Auth.Data;

public class LoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}