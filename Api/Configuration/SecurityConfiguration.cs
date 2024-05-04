namespace Api.Configuration;

public class SecurityConfiguration
{
    public required string SigningKey { get; set; }
    public required string RefreshTokenSecret { get; set; }
}