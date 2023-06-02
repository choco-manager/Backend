using Backend.Data;
using Backend.Modules.Addresses.Contract;
using Backend.Modules.Clients.Contract;
using Backend.Modules.MovementItems.Contract;
using Backend.Modules.MovementStatuses.Contract;

namespace Backend.Modules.Orders.Contract;

public class Order: BaseModel
{
    public DateOnly Date { get; set; }
    public required MovementStatus Status { get; set; }
    public required List<MovementItem> Items { get; set; }
    public required Client Client { get; set; }
    public required Address SelectedAddress { get; set; }
    public bool IsDeleted { get; set; }
}