namespace MigrateData.Data.Models;

public class OldOrderAddress : OldBaseModel {
  public OldOrderCity City { get; set; }
  public string Street { get; set; }
  public string Building { get; set; }
}