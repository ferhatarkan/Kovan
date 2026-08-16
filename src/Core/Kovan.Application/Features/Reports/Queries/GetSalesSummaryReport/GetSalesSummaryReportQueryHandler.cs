using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Reports.Queries.GetSalesSummaryReport;

public class GetSalesSummaryReportQueryHandler : IRequestHandler<GetSalesSummaryReportQuery, List<GetSalesSummaryReportResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public GetSalesSummaryReportQueryHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<List<GetSalesSummaryReportResult>> Handle(GetSalesSummaryReportQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Invoices
            .Where(i => i.Status == InvoiceStatus.Paid || i.Status == InvoiceStatus.PartiallyPaid) // Sadece ödenmiş/kısmen ödenmiş faturaları satış say
            .Where(i => i.IssueDate >= request.StartDate && i.IssueDate <= request.EndDate);

        if (!string.IsNullOrEmpty(request.SalespersonId))
        {
            query = query.Where(i => i.CreatedBy == request.SalespersonId);
        }

        switch (request.GroupBy)
        {
            case SalesReportGrouping.Day:
                return await GetGroupedReport(query, g => g.IssueDate.Date, g => g.Key.ToString("yyyy-MM-dd"), g => g.Key.ToString("dd MMMM yyyy"), cancellationToken);
            case SalesReportGrouping.Month:
                return await GetGroupedReport(query, g => new { g.IssueDate.Year, g.IssueDate.Month }, g => $"{g.Key.Year}-{g.Key.Month:D2}", g => new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy"), cancellationToken);
            case SalesReportGrouping.Year:
                return await GetGroupedReport(query, g => g.IssueDate.Year, g => g.Key.ToString(), g => g.Key.ToString(), cancellationToken);
            case SalesReportGrouping.Salesperson:
            default:
                return await GetSalespersonReport(query, cancellationToken);
        }
    }

    private async Task<List<GetSalesSummaryReportResult>> GetSalespersonReport(IQueryable<Domain.Entities.Invoice> query, CancellationToken cancellationToken)
    {
        var reportData = await query
            .Where(i => i.CreatedBy != null) // Null CreatedBy değerlerini filtrele
            .GroupBy(i => i.CreatedBy!) // Null olamayacağını belirt
            .Select(g => new { GroupKey = g.Key, Aggregates = GetAggregates(g) })
            .ToListAsync(cancellationToken);

        var users = await _identityService.GetUserNamesAsync(reportData.Select(r => r.GroupKey), cancellationToken);

        return reportData.Select(r => new GetSalesSummaryReportResult
        {
            GroupKey = r.GroupKey,
            GroupDisplayName = users.TryGetValue(r.GroupKey, out var name) ? name : "Bilinmeyen Kullanıcı",
            TotalSalesAmount = r.Aggregates.TotalSalesAmount,
            TotalInvoices = r.Aggregates.TotalInvoices,
            TotalProductsSold = r.Aggregates.TotalProductsSold
        }).ToList();
    }

    private async Task<List<GetSalesSummaryReportResult>> GetGroupedReport<TKey>(IQueryable<Domain.Entities.Invoice> query, System.Linq.Expressions.Expression<Func<Domain.Entities.Invoice, TKey>> groupSelector, Func<IGrouping<TKey, Domain.Entities.Invoice>, string> keySelector, Func<IGrouping<TKey, Domain.Entities.Invoice>, string> displayNameSelector, CancellationToken cancellationToken)
    {
        var reportData = await query
            .GroupBy(groupSelector)
            .Select(g => new GetSalesSummaryReportResult
            {
                GroupKey = keySelector(g),
                GroupDisplayName = displayNameSelector(g),
                TotalSalesAmount = g.Sum(i => i.GrandTotal),
                TotalInvoices = g.Count(),
                TotalProductsSold = g.SelectMany(i => i.InvoiceLines).Sum(l => l.Quantity)
            })
            .OrderBy(r => r.GroupKey)
            .ToListAsync(cancellationToken);

        return reportData;
    }

    private static (decimal TotalSalesAmount, int TotalInvoices, int TotalProductsSold) GetAggregates(IGrouping<string, Domain.Entities.Invoice> g)
    {
        return (g.Sum(i => i.GrandTotal), g.Count(), g.SelectMany(i => i.InvoiceLines).Sum(l => l.Quantity));
    }
}