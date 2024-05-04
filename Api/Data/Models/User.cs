using System.ComponentModel.DataAnnotations;
using Api.Data.Common;

namespace Api.Data.Models;

public class User: BaseModel
{
    [MaxLength(10)]
    public required string Login { get; set; }
    [MaxLength(10)]
    public required string Name { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
}