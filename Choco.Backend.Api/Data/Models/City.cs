using Choco.Backend.Api.Data.Common;

namespace Choco.Backend.Api.Data.Models;

public class City : BaseModel
{
    public required string Name { get; set; }
}