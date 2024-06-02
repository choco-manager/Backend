namespace Choco.Backend.Api.Domain.Customers.Data;

public class AddressDto
{
    public Guid Id { get; set; }
    public required string Address { get; set; }
}