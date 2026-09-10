using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Enums;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (customer == null)
        {
            throw new NotFoundException(nameof(Customer), request.Id);
        }

        // İş Kuralı: Müşterinin ödenmemiş veya kısmen ödenmiş faturası varsa silinemez.
        var hasUnpaidInvoices = await _context.Invoices
            .AnyAsync(i => i.CustomerId == request.Id &&
                           (i.Status == InvoiceStatus.Draft || i.Status == InvoiceStatus.PartiallyPaid || i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue), cancellationToken);

        if (hasUnpaidInvoices)
        {
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure("Customer", "Müşterinin ödenmemiş faturaları olduğu için silinemez.") });
        }

        customer.Delete();

        await _context.SaveChangesAsync(cancellationToken);
    }
}