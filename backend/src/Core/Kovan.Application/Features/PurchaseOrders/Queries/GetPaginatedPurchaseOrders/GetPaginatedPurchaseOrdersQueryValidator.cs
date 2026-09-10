using FluentValidation;

namespace Kovan.Application.Features.PurchaseOrders.Queries.GetPaginatedPurchaseOrders;

public class GetPaginatedPurchaseOrdersQueryValidator : AbstractValidator<GetPaginatedPurchaseOrdersQuery>
{
    public GetPaginatedPurchaseOrdersQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Sayfa numarası en az 1 olmalıdır.");
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("Sayfa boyutu en az 1 olmalıdır.");
    }
}