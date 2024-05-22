namespace Choco.Backend.Api.Domain.Auth.Data;

public class RestorationTokenResponse
{
    public required string RestorationToken { get; set; }
    public required string Login { get; set; }
}