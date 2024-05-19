using Api.Data.Common;

namespace Api.Data.Models;

public class Address : BaseModel
{
    public required City City { get; set; }
    public required string Street { get; set; }
    public required string Building { get; set; }
    public int Entrance { get; set; }
}