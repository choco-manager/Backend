namespace MigrateData.Data.Models;

public class OldOrder : OldBaseModel {
  public DateOnly Date { get; set; }
  public OldOrderStatus Status { get; set; }
  public List<OldOrderItem> OrderItems { get; set; }
  public OldOrderAddress Address { get; set; }
  public bool Deleted { get; set; }
}