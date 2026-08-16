using FluentValidation;
using Kovan.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Suppliers.Commands.DeleteSupplier;

public class DeleteSupplierCommandValidator : AbstractValidator<DeleteSupplierCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteSupplierCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Tedarikçi ID'si boş olamaz.")
            .MustAsync(BeUnused).WithMessage("Bu tedarikçiye ait satın alma siparişleri olduğu için silinemez.");
    }
    private async Task<bool> BeUnused(Guid id, CancellationToken cancellationToken)
    {
        return !await _context.PurchaseOrders.AnyAsync(po => po.SupplierId == id, cancellationToken);
    }
}