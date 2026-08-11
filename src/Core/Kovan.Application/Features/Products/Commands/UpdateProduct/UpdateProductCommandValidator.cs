using FluentValidation;

namespace Kovan.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200);
        RuleFor(v => v.Sku).NotEmpty().MaximumLength(50);
        RuleFor(v => v.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Fiyat negatif olamaz.");
    }
}