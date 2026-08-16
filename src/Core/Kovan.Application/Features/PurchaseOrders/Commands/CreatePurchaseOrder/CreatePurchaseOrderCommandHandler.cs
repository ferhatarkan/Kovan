using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreatePurchaseOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        // Sipariş numarasını otomatik oluşturabiliriz veya istekten alabiliriz.
        var orderNumber = $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}";
        var purchaseOrder = PurchaseOrder.Create(request.SupplierId, request.OrderDate, orderNumber);

        foreach (var lineDto in request.Lines)
        {
            purchaseOrder.AddLine(lineDto.ProductId, lineDto.Quantity, lineDto.PurchasePrice, lineDto.VatRate);
        }

        _context.PurchaseOrders.Add(purchaseOrder);
        await _context.SaveChangesAsync(cancellationToken);
        return purchaseOrder.Id;
    }
}