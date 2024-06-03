using Choco.Backend.Api.Data.Common;
using Choco.Backend.Api.Data.Enums;

namespace Choco.Backend.Api.Data.Models;

public class Procurement : BaseModel
{
    public required ICollection<ProcuredProduct> Products { get; set; }
    public DateTime ProcuredAt { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsDeleted { get; set; }
}