using System.ComponentModel.DataAnnotations;

namespace Choco.Backend.Api.Data.Models;

public class RevokedAccessToken
{
    [Key]
    public required string Token { get; set; }
}