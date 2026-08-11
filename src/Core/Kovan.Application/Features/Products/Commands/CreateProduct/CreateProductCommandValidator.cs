using FluentValidation;

namespace Kovan.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200);
        RuleFor(v => v.Sku).NotEmpty().MaximumLength(50);
        RuleFor(v => v.Price).GreaterThanOrEqualTo(0);
    }
}