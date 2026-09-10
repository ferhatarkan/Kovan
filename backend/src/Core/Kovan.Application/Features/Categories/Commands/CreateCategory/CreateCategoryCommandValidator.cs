using FluentValidation;

namespace Kovan.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(100).WithMessage("Kategori adı boş olamaz ve 100 karakterden uzun olamaz.");
        // ParentCategoryId için özel bir validasyon gerekirse eklenebilir (örn: var olan bir kategori mi?)
    }
}