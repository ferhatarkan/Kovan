using FluentValidation;

namespace Kovan.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder;

public class DeletePurchaseOrderCommandValidator : AbstractValidator<DeletePurchaseOrderCommand>
{
    public DeletePurchaseOrderCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Satın alma siparişi ID'si boş olamaz.");
    }
}