using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Invoices.Commands.AddPayment;

public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand>
{
    private readonly IApplicationDbContext _context;

    public AddPaymentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(AddPaymentCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Payments) // Fatura durumunu doğru hesaplamak için ödemeleri dahil et.
            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        if (invoice == null)
        {
            throw new NotFoundException(nameof(Invoice), request.InvoiceId);
        }

        if (request.Amount > invoice.AmountDue)
        {
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure("Amount", $"Ödeme tutarı, kalan borçtan ({invoice.AmountDue:C}) fazla olamaz.") });
        }

        invoice.AddPayment(request.Amount, request.PaymentMethod, request.PaymentDate, request.Notes);

        await _context.SaveChangesAsync(cancellationToken);
    }
}