using Kovan.Domain.Enums;

namespace Kovan.Application.Features.Invoices.Queries.GetAllInvoices;

public class GetAllInvoicesResult
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public decimal GrandTotal { get; set; }
    public InvoiceStatus Status { get; set; }
}