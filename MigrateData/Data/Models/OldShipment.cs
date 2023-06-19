namespace MigrateData.Data.Models;

public class OldShipment : OldBaseModel {
  public DateOnly Date { get; set; }
  public OldShipmentStatus Status { get; set; }
  public List<OldShipmentItem> ShipmentItems { get; set; }
  public bool Deleted { get; set; }
}