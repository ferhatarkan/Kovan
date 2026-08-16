using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using Kovan.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        var invoice = Invoice.Create(request.CustomerId, request.WarehouseId, request.InvoiceNumber, request.DueDate);

        foreach (var lineDto in request.InvoiceLines)
        {
            var product = await _context.Products.FindAsync(new object[] { lineDto.ProductId }, cancellationToken);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product), lineDto.ProductId);
            }

            invoice.AddLine(lineDto.ProductId, lineDto.Quantity, lineDto.UnitPrice, lineDto.VatRate);

            // Stoktan düşme işlemi
            var productWarehouse = await _context.ProductWarehouses
                .FirstOrDefaultAsync(pw => pw.ProductId == lineDto.ProductId && pw.WarehouseId == request.WarehouseId, cancellationToken);

            if (productWarehouse == null) throw new NotFoundException(nameof(ProductWarehouse), lineDto.ProductId);

            productWarehouse.AdjustStock(-lineDto.Quantity); // Stoktan düş
            var transaction = product.CreateInventoryTransaction(request.WarehouseId, -lineDto.Quantity, InventoryTransactionType.Sale, invoice.Id);
            _context.InventoryTransactions.Add(transaction);
        }

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync(cancellationToken);
        return invoice.Id;
    }
}