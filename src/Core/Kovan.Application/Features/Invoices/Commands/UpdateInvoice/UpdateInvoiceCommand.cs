using MediatR;

namespace Kovan.Application.Features.Invoices.Commands.UpdateInvoice;

public class UpdateInvoiceCommand : IRequest
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    // Genellikle faturanın müşterisi değiştirilmez, ancak gerekirse eklenebilir.
    // public Guid CustomerId { get; set; }
    public List<UpdateInvoiceLineItem> Lines { get; set; } = new();
}

public class UpdateInvoiceLineItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatRate { get; set; }
}
