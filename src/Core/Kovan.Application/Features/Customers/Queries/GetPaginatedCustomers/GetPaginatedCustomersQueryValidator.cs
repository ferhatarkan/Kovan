using FluentValidation;

namespace Kovan.Application.Features.Customers.Queries.GetPaginatedCustomers;

public class GetPaginatedCustomersQueryValidator : AbstractValidator<GetPaginatedCustomersQuery>
{
    public GetPaginatedCustomersQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Sayfa numarası en az 1 olmalıdır.");
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("Sayfa boyutu en az 1 olmalıdır.");
    }
}