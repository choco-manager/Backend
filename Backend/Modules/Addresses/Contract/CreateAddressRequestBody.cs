namespace Backend.Modules.Addresses.Contract; 

public class CreateAddressRequestBody {
  public Guid City { get; set; }
  public required string Street { get; set; }
  public required string Building { get; set; }
}