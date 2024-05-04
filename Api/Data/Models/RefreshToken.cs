using System.ComponentModel.DataAnnotations;

namespace Api.Data.Models;

public class RefreshToken
{
    [Key]
    public required string Token { get; set; }
    public required byte[] Salt { get; set; }
    public required User User { get; set; }
    public required DateTime ExpireAt { get; set; }
}