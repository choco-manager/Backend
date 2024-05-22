namespace Choco.Backend.Api.Domain.Auth.Data;

public class RegisterResponse
{
    public required string Login { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}