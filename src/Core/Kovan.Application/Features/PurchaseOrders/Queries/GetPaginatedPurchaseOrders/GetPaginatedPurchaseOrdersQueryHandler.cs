using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Mappings;
using Kovan.Application.Common.Models;
using Kovan.Application.Features.PurchaseOrders.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Kovan.Application.Features.PurchaseOrders.Queries.GetPaginatedPurchaseOrders;

public class GetPaginatedPurchaseOrdersQueryHandler : IRequestHandler<GetPaginatedPurchaseOrdersQuery, PaginatedList<PurchaseOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedPurchaseOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PurchaseOrderDto>> Handle(GetPaginatedPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.PurchaseOrders
            .OrderByDescending(po => po.OrderDate)
            .AsNoTracking();

        // Arama terimi varsa filtrele
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = $"%{request.SearchTerm.ToLower()}%";
            query = query.Where(po => EF.Functions.Like(po.OrderNumber.ToLower(), searchTerm) || // OrderNumber'a göre arama
                                      EF.Functions.Like((po.Supplier != null ? po.Supplier.Name : string.Empty).ToLower(), searchTerm)); // Supplier adı null olabilir
        }

        return await query
            .ProjectTo<PurchaseOrderDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}