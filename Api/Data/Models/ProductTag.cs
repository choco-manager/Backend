using System.ComponentModel.DataAnnotations;
using Api.Data.Common;

namespace Api.Data.Models;

public class ProductTag: BaseModel
{
    [MaxLength(10)]
    public required string Title { get; set; }
    public ICollection<Product> Products { get; set; }
}