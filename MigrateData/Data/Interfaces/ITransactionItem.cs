#region

using MigrateData.Data.Models;

#endregion


namespace MigrateData.Data.Interfaces;

public interface ITransactionItem {
  public OldProduct Product { get; set; }
  public double Amount { get; set; }
}