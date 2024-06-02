namespace Choco.Backend.Api.Domain.Customers.Data;

public class CreateCustomerRequest
{
    public required string Name { get; set; }
    public string? PhoneNumber { get; set; }
    public long? VkId { get; set; }
    public required List<string> DeliveryAddresses { get; set; }
}