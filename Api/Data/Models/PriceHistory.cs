using System.ComponentModel.DataAnnotations.Schema;
using Api.Data.Common;
using Api.Data.Enums;

namespace Api.Data.Models;

[Table("PriceHistory")]
public class PriceHistory : BaseModel
{
    public Guid ProductId { get; set; }
    public DateTime EffectiveTimestamp { get; set; }
    public decimal Price { get; set; }
    public PriceType PriceType { get; set; }
}