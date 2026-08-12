using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.PurchaseOrders.Commands.UpdatePurchaseOrder;

public class UpdatePurchaseOrderCommandHandler : IRequestHandler<UpdatePurchaseOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePurchaseOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(PurchaseOrder), request.Id);

        var productIds = request.Lines.Select(x => x.ProductId).Distinct().ToList();
        var existingProductIds = await _context.Products
            .Where(x => productIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var missingProductId = productIds.Except(existingProductIds).FirstOrDefault();
        if (missingProductId != Guid.Empty)
            throw new NotFoundException(nameof(Product), missingProductId);

        purchaseOrder.Update(request.SupplierId, request.OrderDate, request.OrderNumber);
        purchaseOrder.ClearLines();

        foreach (var line in request.Lines)
            purchaseOrder.AddLine(line.ProductId, line.Quantity, line.PurchasePrice, line.VatRate);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
