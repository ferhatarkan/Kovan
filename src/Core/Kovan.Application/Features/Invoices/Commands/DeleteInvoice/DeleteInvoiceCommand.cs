using MediatR;

namespace Kovan.Application.Features.Invoices.Commands.DeleteInvoice;

public class DeleteInvoiceCommand : IRequest, Kovan.Application.Common.Interfaces.ITransactionalRequest
{
    public Guid Id { get; set; }
}
