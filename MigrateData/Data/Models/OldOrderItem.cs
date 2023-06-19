#region

using System.Text.Json.Serialization;

using MigrateData.Data.Interfaces;

#endregion


namespace MigrateData.Data.Models;

public class OldOrderItem : OldBaseModel, ITransactionItem {
  public OldProduct Product { get; set; }
  public double Amount { get; set; }

  [JsonIgnore]
  public OldOrder? Order { get; set; }
}