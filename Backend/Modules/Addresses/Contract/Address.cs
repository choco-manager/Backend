using Backend.Data;
using Backend.Modules.Cities.Contract;

namespace Backend.Modules.Addresses.Contract;

public class Address: BaseModel
{
    public required City City { get; set; }
    public required string Street { get; set; }
    public required string Building { get; set; }
    
}