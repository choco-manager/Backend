namespace MigrateData.Data.Models;

/// <summary>
///   Базовая модель, от которой наследуются все модели
/// </summary>
public class OldBaseModel {
    /// <summary>
    ///   Идентификатор
    /// </summary>
    public Guid Id { get; set; }
}