using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Data;
using Choco.Backend.Api.Domain.Orders.Data;
using Choco.Backend.Api.Domain.Orders.Endpoints.UpdatePaymentStatus;
using Microsoft.EntityFrameworkCore;

namespace Choco.Backend.Api.Domain.Orders.UseCases;

public class UpdatePaymentStatusUseCase(AppDbContext db) : IUseCase<UpdatePaymentStatusRequest, IdModel>
{
    public async Task<Result<IdModel>> Execute(UpdatePaymentStatusRequest req, CancellationToken ct = default)
    {
        var order = await db.Orders
            .Where(e => e.Id == req.Id)
            .FirstOrDefaultAsync(ct);

        if (order is null)
        {
            return Result.NotFound();
        }

        order.PaymentStatus = req.Status;

        await db.SaveChangesAsync(ct);

        return Result.Success(new IdModel() { Id = order.Id });
    }
}