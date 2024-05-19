using Api.Data.Common;

namespace Api.Data.Models;

public class City : BaseModel
{
    public required string Name { get; set; }
}