using FluentValidation;

namespace Kovan.Application.Features.Suppliers.Queries.GetPaginatedSuppliers;

public class GetPaginatedSuppliersQueryValidator : AbstractValidator<GetPaginatedSuppliersQuery>
{
    public GetPaginatedSuppliersQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Sayfa numarası en az 1 olmalıdır.");
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("Sayfa boyutu en az 1 olmalıdır.");
    }
}