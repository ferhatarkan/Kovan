using FluentValidation;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder;

public class DeletePurchaseOrderCommandValidator : AbstractValidator<DeletePurchaseOrderCommand>
{
    private readonly IApplicationDbContext _context;
    public DeletePurchaseOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Satın alma siparişi ID'si boş olamaz.")
            .MustAsync(CanBeDeleted).WithMessage("Teslim alınmış veya tamamlanmış bir satın alma siparişi silinemez.");
    }

    private async Task<bool> CanBeDeleted(Guid id, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(new object[] { id }, cancellationToken);
        return purchaseOrder != null && purchaseOrder.Status != PurchaseOrderStatus.Completed;
    }
}