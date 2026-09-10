using FluentValidation;
using Kovan.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderCommandValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public CreatePurchaseOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.SupplierId)
            .NotEmpty().WithMessage("Tedarikçi ID'si boş olamaz.")
            .MustAsync(SupplierMustExist).WithMessage("Belirtilen tedarikçi bulunamadı.");

        RuleFor(v => v.Lines)
            .NotEmpty().WithMessage("Satın alma siparişi en az bir satır içermelidir.");

        RuleForEach(v => v.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductId).NotEmpty().WithMessage("Ürün ID'si boş olamaz.")
                .MustAsync(ProductMustExist).WithMessage("Satırdaki ürün bulunamadı.");
            line.RuleFor(l => l.Quantity).GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır.");
            line.RuleFor(l => l.PurchasePrice).GreaterThanOrEqualTo(0).WithMessage("Satın alma fiyatı negatif olamaz.");
        });
    }

    private async Task<bool> SupplierMustExist(Guid supplierId, CancellationToken cancellationToken) => await _context.Suppliers.AnyAsync(s => s.Id == supplierId, cancellationToken);
    private async Task<bool> ProductMustExist(Guid productId, CancellationToken cancellationToken) => await _context.Products.AnyAsync(p => p.Id == productId, cancellationToken);
}