using FluentValidation;
using Kovan.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateInvoiceCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("Müşteri ID'si boş olamaz.")
            .MustAsync(CustomerMustExist).WithMessage("Belirtilen müşteri bulunamadı.");

        RuleFor(v => v.WarehouseId)
            .NotEmpty().WithMessage("Depo ID'si boş olamaz.")
            .MustAsync(WarehouseMustExist).WithMessage("Belirtilen depo bulunamadı.");

        RuleFor(v => v.InvoiceLines)
            .NotEmpty().WithMessage("Fatura en az bir satır içermelidir.");

        RuleForEach(v => v.InvoiceLines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductId).NotEmpty().WithMessage("Ürün ID'si boş olamaz.")
                .MustAsync(ProductMustExist).WithMessage("Satırdaki ürün bulunamadı.");
            line.RuleFor(l => l.Quantity).GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır.");
            line.RuleFor(l => l.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Birim fiyat negatif olamaz.");
        });

        RuleForEach(v => v.InvoiceLines)
            .MustAsync(HaveEnoughStock)
            .WithMessage((cmd, line) => $"'{line.ProductId}' ID'li ürün için yeterli stok bulunmamaktadır.");
    }

    private async Task<bool> CustomerMustExist(Guid customerId, CancellationToken cancellationToken) => await _context.Customers.AnyAsync(c => c.Id == customerId, cancellationToken);
    private async Task<bool> WarehouseMustExist(Guid warehouseId, CancellationToken cancellationToken) => await _context.Warehouses.AnyAsync(w => w.Id == warehouseId, cancellationToken); // Bu satır zaten vardı, bağlam için bırakıldı.
    private async Task<bool> ProductMustExist(Guid productId, CancellationToken cancellationToken) => await _context.Products.AnyAsync(p => p.Id == productId, cancellationToken);

    private async Task<bool> HaveEnoughStock(CreateInvoiceCommand command, CreateInvoiceLineDto line, CancellationToken cancellationToken)
    {
        var productWarehouse = await _context.ProductWarehouses
            .FirstOrDefaultAsync(pw => pw.ProductId == line.ProductId && pw.WarehouseId == command.WarehouseId, cancellationToken);

        return productWarehouse != null && productWarehouse.StockQuantity >= line.Quantity;
    }
}