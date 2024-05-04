using System.ComponentModel.DataAnnotations;

namespace Api.Data.Models;

public class RevokedAccessToken
{
    [Key]
    public required string Token { get; set; }
}