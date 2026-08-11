using MediatR;

namespace Kovan.Application.Features.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommand : IRequest<Guid>
{
    public Guid CustomerId { get; init; }
    public string InvoiceNumber { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
    public List<InvoiceLineItem> Lines { get; init; } = new();

    public class InvoiceLineItem
    {
        public Guid ProductId { get; init; }
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal VatRate { get; init; }
    }
}
