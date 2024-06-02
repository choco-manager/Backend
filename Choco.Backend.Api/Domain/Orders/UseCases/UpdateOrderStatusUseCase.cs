using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.Orders.Data;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Orders.UseCases;

public class UpdateOrderStatusUseCase(AppDbContext db) : IUseCase<UpdateOrderStatusRequest, IdModel>
{
    public async Task<Result<IdModel>> Execute(UpdateOrderStatusRequest req, CancellationToken ct = default)
    {
        var order = await db.Orders
            .Where(e => e.Id == req.Id)
            .FirstOrDefaultAsync(ct);

        if (order is null)
        {
            return Result.NotFound();
        }

        order.OrderStatus = req.Status;

        await db.SaveChangesAsync(ct);

        return Result.Success(new IdModel() { Id = order.Id });
    }
}