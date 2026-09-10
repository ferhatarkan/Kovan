using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.PurchaseOrders.Queries.GetPaginatedPurchaseOrders;

public class GetPaginatedPurchaseOrdersQueryHandler : IRequestHandler<GetPaginatedPurchaseOrdersQuery, PaginatedList<GetPaginatedPurchaseOrdersResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedPurchaseOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedPurchaseOrdersResult>> Handle(GetPaginatedPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        return await PaginatedList<GetPaginatedPurchaseOrdersResult>.CreateAsync(
            _context.PurchaseOrders.AsNoTracking().Include(p => p.Supplier).OrderByDescending(p => p.OrderDate)
                .ProjectTo<GetPaginatedPurchaseOrdersResult>(_mapper.ConfigurationProvider),
            request.PageNumber, request.PageSize, cancellationToken);
    }
}