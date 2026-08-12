using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Enums;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateInvoiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        // 1. Müşterinin varlığını kontrol et
        var customerExists = await _context.Customers.AnyAsync(c => c.Id == request.CustomerId, cancellationToken);
        if (!customerExists)
        {
            throw new NotFoundException(nameof(Customer), request.CustomerId);
        }

        var warehouseExists = await _context.Warehouses.AnyAsync(w => w.Id == request.WarehouseId, cancellationToken);
        if (!warehouseExists)
            throw new NotFoundException(nameof(Warehouse), request.WarehouseId);

        // 2. Fatura başlığını oluştur
        var invoice = Invoice.Create(request.CustomerId, request.WarehouseId, request.InvoiceNumber, request.DueDate);

        // 3. Performans iyileştirmesi: Gerekli tüm ürünleri tek bir sorgu ile çek.
        var productIds = request.Lines.Select(l => l.ProductId).Distinct().ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p, cancellationToken);
        var stockByProductId = await _context.ProductWarehouses
            .Where(x => x.WarehouseId == request.WarehouseId && productIds.Contains(x.ProductId))
            .ToDictionaryAsync(x => x.ProductId, cancellationToken);

        // 4. Fatura satırlarını işle
        foreach (var lineItem in request.Lines)
        {
            // Ürünü veritabanından değil, önceden doldurduğumuz sözlükten al.
            if (!products.TryGetValue(lineItem.ProductId, out var product))
            {
                throw new NotFoundException(nameof(Product), lineItem.ProductId);
            }

            if (!stockByProductId.TryGetValue(lineItem.ProductId, out var productWarehouse))
                throw new NotFoundException(nameof(ProductWarehouse), lineItem.ProductId);

            // Stok kontrolü ve düşümü
            // Not: UpdateStock metodu, yetersiz stok durumunda bir exception fırlatmalıdır.
            // Bu, domain katmanında ele alınması gereken bir iş kuralıdır.
            // Product entity'si artık doğrudan stok güncellemesi yapmıyor.
            // Gerçek stok güncellemesi ProductWarehouse üzerinden yapılmalı ve WarehouseId bilgisi gereklidir.
            productWarehouse.AdjustStock(-lineItem.Quantity);
            var transaction = product.CreateInventoryTransaction(request.WarehouseId, -lineItem.Quantity, InventoryTransactionType.Sale, invoice.Id);
            _context.InventoryTransactions.Add(transaction);


            // Fatura satırını domaine ekle (toplamlar otomatik hesaplanacak)
            invoice.AddLine(product.Id, lineItem.Quantity, lineItem.UnitPrice, lineItem.VatRate);
        }

        _context.Invoices.Add(invoice); // AddAsync yerine Add kullanıldı.
        await _context.SaveChangesAsync(cancellationToken);

        return invoice.Id;
    }
}
