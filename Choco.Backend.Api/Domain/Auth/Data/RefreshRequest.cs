namespace Choco.Backend.Api.Domain.Auth.Data;

public class RefreshRequest
{
    public required string RefreshToken { get; set; }
}