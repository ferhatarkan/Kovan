using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Dashboard.Queries;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardDto>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardStatsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var dashboardDto = new DashboardDto
        {
            TotalInvoices = await _context.Invoices.CountAsync(cancellationToken),
            TotalCustomers = await _context.Customers.CountAsync(cancellationToken),
            TotalProducts = await _context.Products.CountAsync(cancellationToken),
            TotalRevenue = await _context.Payments.SumAsync(p => p.Amount, cancellationToken),

            InvoiceStatusSummary = new InvoiceStatusSummary
            {
                Paid = await _context.Invoices.Where(i => i.Status == InvoiceStatus.Paid).CountAsync(cancellationToken),
                PartiallyPaid = await _context.Invoices.Where(i => i.Status == InvoiceStatus.PartiallyPaid).CountAsync(cancellationToken),
                Draft = await _context.Invoices.Where(i => i.Status == InvoiceStatus.Draft).CountAsync(cancellationToken)
            },

            TopSellingProducts = await _context.InvoiceLines
                .GroupBy(l => l.Product!.Name)
                .Select(g => new TopSellingProductDto
                {
                    ProductName = g.Key,
                    TotalQuantitySold = g.Sum(l => l.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantitySold)
                .Take(5) // En çok satan ilk 5 ürünü al
                .ToListAsync(cancellationToken)
        };

        return dashboardDto;
    }
}