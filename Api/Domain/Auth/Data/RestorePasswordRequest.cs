namespace Api.Domain.Auth.Data;

public class RestorePasswordRequest
{
    public required string RestorationToken { get; set; }
    public required string NewPassword { get; set; }
}