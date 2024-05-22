namespace Choco.Backend.Api.Configuration;

public class SecurityConfiguration
{
    public required string SigningKey { get; set; }
    public required string RefreshTokenSecret { get; set; }
    public required string RestorationTokenSecret { get; set; }
}