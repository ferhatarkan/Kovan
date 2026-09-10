using FluentValidation;

namespace Kovan.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Ürün ID'si boş olamaz.");
    }
}