using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Enums;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Invoices.Commands.DeleteInvoice;

public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteInvoiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
            .Include(i => i.InvoiceLines) // Silme işlemi için satırları da dahil etmeliyiz.
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice == null)
        {
            throw new NotFoundException(nameof(Invoice), request.Id);
        }

        // --- STOK İADE İŞLEMİ ---
        // Fatura silinmeden önce, fatura satırlarındaki ürünleri stoğa geri ekle.
        var productIds = invoice.InvoiceLines.Select(l => l.ProductId).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);
        var stockByProductId = await _context.ProductWarehouses
            .Where(x => x.WarehouseId == invoice.WarehouseId && productIds.Contains(x.ProductId))
            .ToDictionaryAsync(x => x.ProductId, cancellationToken);

        foreach (var line in invoice.InvoiceLines)
        {
            if (products.TryGetValue(line.ProductId, out var product) && stockByProductId.TryGetValue(line.ProductId, out var productWarehouse))
            {
                // Fatura silindiğinde ürünleri stoğa geri ekle ve InventoryTransaction kaydı oluştur.
                // Not: Product entity'si artık doğrudan stok güncellemesi yapmıyor.
                // Gerçek stok güncellemesi ProductWarehouse üzerinden yapılmalı ve WarehouseId bilgisi gereklidir.
                productWarehouse.AdjustStock(line.Quantity);
                var transaction = product.CreateInventoryTransaction(invoice.WarehouseId, line.Quantity, InventoryTransactionType.Return, invoice.Id);
                _context.InventoryTransactions.Add(transaction);
            }
            else
                throw new NotFoundException(nameof(ProductWarehouse), line.ProductId);
        }

        invoice.Delete(); // Domain'deki metodu çağırarak soft delete yap.
        await _context.SaveChangesAsync(cancellationToken);
    }
}
