#region

using MigrateData.Data.Models;

#endregion


namespace MigrateData.Data.Interfaces;

public interface ITransactionItem {
  public OldProduct OldProduct { get; set; }
  public double Amount { get; set; }
}