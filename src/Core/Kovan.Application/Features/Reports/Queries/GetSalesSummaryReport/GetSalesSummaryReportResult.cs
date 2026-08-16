namespace Kovan.Application.Features.Reports.Queries.GetSalesSummaryReport;

public class GetSalesSummaryReportResult
{
    public string GroupKey { get; set; } = string.Empty; // SalespersonId, "2023-10-26", "2023-10", "2023" olabilir
    public string GroupDisplayName { get; set; } = string.Empty; // "John Doe", "26 Ekim 2023", "Ekim 2023", "2023" olabilir
    public int TotalInvoices { get; set; }
    public int TotalProductsSold { get; set; }
    public decimal TotalSalesAmount { get; set; }
}