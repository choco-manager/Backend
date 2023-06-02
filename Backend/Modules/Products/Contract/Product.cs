using Backend.Data;
using Backend.Modules.ProductCategories.Contract;

namespace Backend.Modules.Products.Contract;

public class Product: BaseModel
{
    public required string Name { get; set; }
    public required ProductCategory Category { get; set; }
    public bool IsByWeight { get; set; }
    public bool IsDeleted { get; set; }
    public int VkMarketId { get; set; }
}