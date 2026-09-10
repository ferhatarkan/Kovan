using FluentValidation;
using Kovan.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id).NotEmpty().WithMessage("Kategori ID'si boş olamaz.");
        RuleFor(v => v.Name).NotEmpty().MaximumLength(100).WithMessage("Kategori adı boş olamaz ve 100 karakterden uzun olamaz.");

        RuleFor(v => v.ParentCategoryId)
            .Must((command, parentId) => parentId != command.Id)
            .WithMessage("Kategori kendi kendisinin üst kategorisi olamaz.")
            .When(v => v.ParentCategoryId.HasValue);

        RuleFor(v => v.ParentCategoryId)
            .MustAsync(ParentCategoryMustExist)
            .WithMessage("Belirtilen üst kategori bulunamadı.")
            .When(v => v.ParentCategoryId.HasValue);
    }

    private async Task<bool> ParentCategoryMustExist(Guid? parentCategoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories.AnyAsync(c => c.Id == parentCategoryId, cancellationToken);
    }
}