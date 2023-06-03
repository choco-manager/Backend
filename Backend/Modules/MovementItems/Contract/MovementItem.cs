using Backend.Data;
using Backend.Modules.Products.Contract;


namespace Backend.Modules.MovementItems.Contract;

public class MovementItem : BaseModel {
  public required Product Product { get; set; }
  public decimal Amount { get; set; }
}