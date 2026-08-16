using FluentValidation;

namespace Kovan.Application.Features.Categories.Queries.GetPaginatedCategories;

public class GetPaginatedCategoriesQueryValidator : AbstractValidator<GetPaginatedCategoriesQuery>
{
    public GetPaginatedCategoriesQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Sayfa numarası en az 1 olmalıdır.");
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("Sayfa boyutu en az 1 olmalıdır.");
    }
}