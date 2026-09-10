using Kovan.Domain.Enums;

namespace Kovan.Application.Features.Invoices.Queries.GetInvoiceById;

public class GetInvoiceByIdResult
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public InvoiceStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalVatAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountDue { get; set; }
    public List<InvoiceLineItem> InvoiceLines { get; set; } = new();
}

public class InvoiceLineItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal GrossTotal { get; set; }
}