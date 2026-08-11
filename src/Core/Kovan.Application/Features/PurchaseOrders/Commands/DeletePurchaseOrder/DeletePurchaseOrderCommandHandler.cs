using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using Kovan.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder;

public class DeletePurchaseOrderCommandHandler : IRequestHandler<DeletePurchaseOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePurchaseOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .FirstOrDefaultAsync(po => po.Id == request.Id, cancellationToken);

        if (purchaseOrder == null)
        {
            throw new NotFoundException(nameof(PurchaseOrder), request.Id);
        }

        // İş Kuralı: Sadece 'Taslak' durumundaki siparişler silinebilir.
        if (purchaseOrder.Status != PurchaseOrderStatus.Draft)
        {
            throw new BadRequestException("Sadece 'Taslak' durumundaki satın alma siparişleri silinebilir.");
        }

        // Hard delete yerine soft delete uygula.
        purchaseOrder.Delete();
        await _context.SaveChangesAsync(cancellationToken);
    }
}