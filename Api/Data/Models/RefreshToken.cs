using System.ComponentModel.DataAnnotations;

namespace Api.Data.Models;

public class RefreshToken
{
    [Key]
    public required Guid UserId { get; set; }

    public required string Token { get; set; }

    public required byte[] Salt { get; set; }
    public required DateTime ExpireAt { get; set; }
}