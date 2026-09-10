using FluentValidation;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Invoices.Commands.DeleteInvoice;

public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteInvoiceCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Fatura ID'si boş olamaz.")
            .MustAsync(CanBeDeleted).WithMessage("Ödenmiş veya kısmen ödenmiş faturalar silinemez.");
    }
    private async Task<bool> CanBeDeleted(Guid id, CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices.FindAsync(new object[] { id }, cancellationToken);
        return invoice != null && invoice.Status != InvoiceStatus.Paid && invoice.Status != InvoiceStatus.PartiallyPaid;
    }
}