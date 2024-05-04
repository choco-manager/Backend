namespace Api.Domain.Auth.Data;

public class RegisterRequest
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}