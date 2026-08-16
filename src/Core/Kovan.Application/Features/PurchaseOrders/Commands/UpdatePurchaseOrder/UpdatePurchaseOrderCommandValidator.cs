using FluentValidation;

namespace Kovan.Application.Features.PurchaseOrders.Commands.UpdatePurchaseOrder;

public class UpdatePurchaseOrderCommandValidator : AbstractValidator<UpdatePurchaseOrderCommand>
{
    public UpdatePurchaseOrderCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Satın alma siparişi ID'si boş olamaz.");
    }
}