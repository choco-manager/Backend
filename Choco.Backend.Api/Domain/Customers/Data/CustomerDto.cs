namespace Choco.Backend.Api.Domain.Customers.Data;

public class CustomerDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required List<AddressDto> ShippingAddresses { get; set; }
}