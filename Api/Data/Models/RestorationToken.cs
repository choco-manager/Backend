using Api.Data.Common;

namespace Api.Data.Models;

public class RestorationToken : BaseModel
{
    public required string Token { get; set; }
    public required byte[] Salt { get; set; }
    public bool IsValid { get; set; }
    public DateTime ValidUntil { get; set; }
    public required string Login { get; set; }
}