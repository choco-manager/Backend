using Backend.Data;
using Backend.Modules.PriceTypes.Contract;
using Backend.Modules.Products.Contract;

namespace Backend.Modules.PriceChanges.Contract;

public class PriceChange: BaseModel
{
    public required Product Product { get; set; }
    public required PriceType PriceType { get; set; }
    public DateTime ChangeTimestamp { get; set; }
    public int Price { get; set; }
}