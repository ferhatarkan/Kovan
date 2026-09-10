using FluentValidation;
using Kovan.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Kategori ID'si boş olamaz.")
            .MustAsync(BeUnused).WithMessage("Bu kategori, ürünler tarafından kullanıldığı için silinemez.")
            .MustAsync(NotHaveChildren).WithMessage("Bu kategorinin alt kategorileri olduğu için silinemez.");
    }

    private async Task<bool> BeUnused(Guid categoryId, CancellationToken cancellationToken)
    {
        return !await _context.Products.AnyAsync(p => p.CategoryId == categoryId, cancellationToken);
    }
    private async Task<bool> NotHaveChildren(Guid categoryId, CancellationToken cancellationToken)
    {
        return !await _context.Categories.AnyAsync(c => c.ParentCategoryId == categoryId, cancellationToken);
    }
}