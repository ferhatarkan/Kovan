using FluentValidation;

namespace Kovan.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Ürün ID'si boş olamaz.");
    }
}