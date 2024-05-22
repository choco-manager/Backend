using System.ComponentModel.DataAnnotations;

namespace Choco.Backend.Api.Data.Models;

public class FcmToken
{
    [Key]
    public required Guid UserId { get; set; }
    public required string Token { get; set; }
}