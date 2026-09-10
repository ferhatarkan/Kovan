using FluentValidation;

namespace Kovan.Application.Features.Invoices.Queries.GetPaginatedInvoices;

public class GetPaginatedInvoicesQueryValidator : AbstractValidator<GetPaginatedInvoicesQuery>
{
    public GetPaginatedInvoicesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Sayfa numarası en az 1 olmalıdır.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Sayfa boyutu 1 ile 100 arasında olmalıdır.");
    }
}