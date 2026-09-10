using MediatR;
using System;
using System.Collections.Generic;

namespace Kovan.Application.Features.Reports.Queries.GetSalesSummaryReport;

public class GetSalesSummaryReportQuery : IRequest<List<GetSalesSummaryReportResult>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? SalespersonId { get; set; } // Opsiyonel: Belirli bir satış danışmanına göre filtrelemek için
    public SalesReportGrouping GroupBy { get; set; } = SalesReportGrouping.Salesperson; // Yeni gruplama parametresi
}