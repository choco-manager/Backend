using Api.Data.Common;

namespace Api.Data.Models;

public class Product : BaseModel
{
    public required string Title { get; set; }
    public required ICollection<ProductTag> Tags { get; set; }
    public required int StockBalance { get; set; }
    public required bool IsBulk { get; set; }
    public required bool IsDeleted { get; set; }
    public ICollection<PriceHistory> Prices { get; set; }
}