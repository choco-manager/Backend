using Backend.Data;
using Backend.Modules.Addresses.Contract;

namespace Backend.Modules.Clients.Contract;

public class Client: BaseModel
{
    public required string FirstName { get; set; }
    public required string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ChatLink { set; get; }
    public required List<Address> Addresses { get; set; }
}