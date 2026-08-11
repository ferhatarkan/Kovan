namespace Kovan.Application.Features.Invoices.Queries;

public class InvoiceDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalVatAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public List<InvoiceLineDto> InvoiceLines { get; set; } = new();
}

public class InvoiceLineDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatRate { get; set; }
    public decimal NetTotal { get; set; }
    public decimal VatAmount { get; set; }
    public decimal GrossTotal { get; set; }
}
