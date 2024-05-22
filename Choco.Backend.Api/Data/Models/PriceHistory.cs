using System.ComponentModel.DataAnnotations.Schema;
using Choco.Backend.Api.Data.Common;
using Choco.Backend.Api.Data.Enums;

namespace Choco.Backend.Api.Data.Models;

[Table("PriceHistory")]
public class PriceHistory : BaseModel
{
    public Guid ProductId { get; set; }
    public DateTime EffectiveTimestamp { get; set; }
    public decimal Price { get; set; }
    public PriceType PriceType { get; set; }
}