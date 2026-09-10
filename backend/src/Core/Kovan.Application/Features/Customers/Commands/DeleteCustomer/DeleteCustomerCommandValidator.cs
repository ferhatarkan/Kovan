using FluentValidation;
using Kovan.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCustomerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Müşteri ID'si boş olamaz.")
            .MustAsync(BeUnused).WithMessage("Bu müşterinin faturaları olduğu için silinemez.");
    }

    private async Task<bool> BeUnused(Guid id, CancellationToken cancellationToken) =>
        !await _context.Invoices.AnyAsync(i => i.CustomerId == id, cancellationToken);
}