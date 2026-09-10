using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Enums;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Invoices.Commands.UpdateInvoice;

public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateInvoiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
            .Include(i => i.InvoiceLines) // Satırları güncellemek için dahil etmeliyiz.
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice == null)
        {
            throw new NotFoundException(nameof(Invoice), request.Id);
        }

        // --- STOK İADE İŞLEMİ ---
        // 1. Güncellenecek ürünlerin ID'lerini ve eski miktarlarını topla.
        var oldLineItems = invoice.InvoiceLines.ToList();
        var productIdsToUpdate = oldLineItems.Select(l => l.ProductId)
            .Union(request.Lines.Select(l => l.ProductId))
            .Distinct()
            .ToList();

        var products = await _context.Products
            .Where(p => productIdsToUpdate.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);
        var stockByProductId = await _context.ProductWarehouses
            .Where(x => x.WarehouseId == invoice.WarehouseId && productIdsToUpdate.Contains(x.ProductId))
            .ToDictionaryAsync(x => x.ProductId, cancellationToken);

        // 2. Eski satırlardaki ürünleri stoğa geri ekle.
        foreach (var oldLine in oldLineItems)
        {
            if (products.TryGetValue(oldLine.ProductId, out var product) && stockByProductId.TryGetValue(oldLine.ProductId, out var productWarehouse))
            {
                // Fatura güncellenirken eski satırları stoğa iade et ve 'Return' tipinde transaction kaydı oluştur.
                // Not: Product entity'si artık doğrudan stok güncellemesi yapmıyor.
                // Gerçek stok güncellemesi ProductWarehouse üzerinden yapılmalı ve WarehouseId bilgisi gereklidir.
                productWarehouse.AdjustStock(oldLine.Quantity);
                var transaction = product.CreateInventoryTransaction(invoice.WarehouseId, oldLine.Quantity, InventoryTransactionType.Return, invoice.Id);
                _context.InventoryTransactions.Add(transaction);
            }
            else
            {
                throw new NotFoundException(nameof(ProductWarehouse), oldLine.ProductId);
            }
        }

        invoice.Update(request.InvoiceNumber, request.DueDate);
        invoice.ClearLines(); // Mevcut satırları temizle

        foreach (var lineItem in request.Lines)
        {
            // 3. Yeni satırlardaki ürünleri stoktan düş.
            if (products.TryGetValue(lineItem.ProductId, out var product) && stockByProductId.TryGetValue(lineItem.ProductId, out var productWarehouse))
            {
                // Yeni satırları stoktan düş ve 'Sale' tipinde transaction kaydı oluştur.
                // Not: Product entity'si artık doğrudan stok güncellemesi yapmıyor.
                // Gerçek stok güncellemesi ProductWarehouse üzerinden yapılmalı ve WarehouseId bilgisi gereklidir.
                productWarehouse.AdjustStock(-lineItem.Quantity);
                var transaction = product.CreateInventoryTransaction(invoice.WarehouseId, -lineItem.Quantity, InventoryTransactionType.Sale, invoice.Id);
                _context.InventoryTransactions.Add(transaction);
                invoice.AddLine(lineItem.ProductId, lineItem.Quantity, lineItem.UnitPrice, lineItem.VatRate);
            }
            else
            {
                // Bu durum, validator'dan geçmemesi gereken bir durumu yakalar.
                throw new NotFoundException(nameof(ProductWarehouse), lineItem.ProductId);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
