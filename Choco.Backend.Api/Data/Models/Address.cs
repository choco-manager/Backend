using Choco.Backend.Api.Data.Common;

namespace Choco.Backend.Api.Data.Models;

public class Address : BaseModel
{
    public required City City { get; set; }
    public required string Street { get; set; }
    public required string Building { get; set; }
    public int Entrance { get; set; }

    public override string ToString()
    {
        return $"{City.Name}, {Street}, {Building}";
    }
}