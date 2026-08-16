namespace Kovan.Application.Features.Invoices.Commands.CreateInvoice;

public class CreateInvoiceLineDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatRate { get; set; }
}