using FluentValidation;

namespace Kovan.Application.Features.Products.Queries.GetProductLabel;

public class GetProductLabelQueryValidator : AbstractValidator<GetProductLabelQuery>
{
    public GetProductLabelQueryValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Ürün ID'si boş olamaz.");
    }
}