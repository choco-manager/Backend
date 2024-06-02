using Choco.Backend.Api.Data.Common;

namespace Choco.Backend.Api.Data.Models;

public class Customer: BaseModel
{
    public required string Name { get; set; }
    public required ICollection<Address> ShippingAddresses { get; set; }
    public string? PhoneNumber { get; set; }
    public long? VkId { get; set; }
}