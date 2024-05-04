using System.ComponentModel.DataAnnotations;

namespace Api.Data.Models;

public class FcmToken
{
    [Key]
    public required Guid UserId { get; set; }
    public required string Token { get; set; }
}