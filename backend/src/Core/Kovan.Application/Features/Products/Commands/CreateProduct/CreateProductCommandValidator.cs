using FluentValidation;

namespace Kovan.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200).WithMessage("Ürün adı boş olamaz ve 200 karakterden uzun olamaz.");
        RuleFor(v => v.Sku).NotEmpty().MaximumLength(50).WithMessage("Ürün kodu (SKU) boş olamaz ve 50 karakterden uzun olamaz.");
        RuleFor(v => v.Price).GreaterThanOrEqualTo(0).WithMessage("Fiyat negatif olamaz.");
        RuleFor(v => v.CategoryId).NotEmpty().WithMessage("Kategori ID'si boş olamaz."); // Yeni kural
        RuleFor(v => v.Brand).NotEmpty().MaximumLength(100).WithMessage("Marka adı boş olamaz ve 100 karakterden uzun olamaz."); // Yeni kural
        // Properties için özel validasyonlar eklenebilir.
    }
}