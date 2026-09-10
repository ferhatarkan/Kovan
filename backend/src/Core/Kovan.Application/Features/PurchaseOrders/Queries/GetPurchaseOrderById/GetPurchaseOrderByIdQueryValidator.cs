using FluentValidation;

namespace Kovan.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdQueryValidator : AbstractValidator<GetPurchaseOrderByIdQuery>
{
    public GetPurchaseOrderByIdQueryValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Satın alma siparişi ID'si boş olamaz.");
    }
}