using Backend.Data;
using Backend.Modules.MovementItems.Contract;
using Backend.Modules.MovementStatuses.Contract;


namespace Backend.Modules.Shipments.Contract;

public class Shipment : BaseModel {
  public DateOnly Date { get; set; }
  public required MovementStatus Status { get; set; }
  public required List<MovementItem> Items { get; set; }
  public bool IsDeleted { get; set; }
}