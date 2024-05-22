using System.ComponentModel.DataAnnotations;
using Choco.Backend.Api.Data.Common;

namespace Choco.Backend.Api.Data.Models;

public class ProductTag: BaseModel
{
    [MaxLength(10)]
    public required string Title { get; set; }
    public ICollection<Product> Products { get; set; }
}