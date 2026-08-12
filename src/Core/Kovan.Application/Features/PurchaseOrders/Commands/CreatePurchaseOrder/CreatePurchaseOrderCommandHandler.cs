using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using Kovan.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var supplierExists = await _context.Suppliers.AnyAsync(x => x.Id == request.SupplierId, cancellationToken);
        if (!supplierExists)
            throw new NotFoundException(nameof(Supplier), request.SupplierId);

        // 1. Satın alma siparişini oluştur
        var purchaseOrder = PurchaseOrder.Create(request.SupplierId, request.OrderDate, request.OrderNumber);

        // 2. Satırları işle ve stokları güncelle
        foreach (var lineItem in request.Lines)
        {
            var product = await _context.Products.FindAsync(new object[] { lineItem.ProductId }, cancellationToken)
                ?? throw new NotFoundException(nameof(Product), lineItem.ProductId);

            // Satın alma siparişine satırı ekle
            purchaseOrder.AddLine(lineItem.ProductId, lineItem.Quantity, lineItem.PurchasePrice, lineItem.VatRate);

            // TODO: Stok yönetimi artık çoklu depo desteği ile ayrı bir komutla yapılmalıdır.
            // Örneğin: await _mediator.Send(new AdjustStockCommand { ... });
        }

        // 4. Satın alma siparişini veritabanına ekle
        _context.PurchaseOrders.Add(purchaseOrder);

        // 5. Tüm değişiklikleri tek bir işlemde kaydet
        await _context.SaveChangesAsync(cancellationToken);

        return purchaseOrder.Id;
    }
}
