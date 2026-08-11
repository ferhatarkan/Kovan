using MediatR;

namespace Kovan.Application.Features.Invoices.Commands.DeleteInvoice;

public class DeleteInvoiceCommand : IRequest
{
    public Guid Id { get; set; }
}
