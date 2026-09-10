using FluentValidation;

namespace Kovan.Application.Features.Products.Queries.GetPaginatedProducts;

public class GetPaginatedProductsQueryValidator : AbstractValidator<GetPaginatedProductsQuery>
{
    public GetPaginatedProductsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Sayfa numarası en az 1 olmalıdır.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Sayfa boyutu 1 ile 100 arasında olmalıdır.");
    }
}