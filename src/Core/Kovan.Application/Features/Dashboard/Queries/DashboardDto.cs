namespace Kovan.Application.Features.Dashboard.Queries;

public class DashboardDto
{
    public int TotalInvoices { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalProducts { get; set; }
    public decimal TotalRevenue { get; set; }
    public InvoiceStatusSummary InvoiceStatusSummary { get; set; } = new();
    public List<TopSellingProductDto> TopSellingProducts { get; set; } = new();
}

public class InvoiceStatusSummary
{
    public int Paid { get; set; }
    public int PartiallyPaid { get; set; }
    public int Draft { get; set; }
}

public class TopSellingProductDto
{
    public string ProductName { get; set; } = string.Empty;
    public int TotalQuantitySold { get; set; }
}