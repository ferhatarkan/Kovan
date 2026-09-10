using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Suppliers.Queries.GetPaginatedSuppliers;

public class GetPaginatedSuppliersQueryHandler : IRequestHandler<GetPaginatedSuppliersQuery, PaginatedList<GetPaginatedSuppliersResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedSuppliersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedSuppliersResult>> Handle(GetPaginatedSuppliersQuery request, CancellationToken cancellationToken)
    {
        return await PaginatedList<GetPaginatedSuppliersResult>.CreateAsync(
            _context.Suppliers.OrderBy(s => s.Name).ProjectTo<GetPaginatedSuppliersResult>(_mapper.ConfigurationProvider),
            request.PageNumber, request.PageSize, cancellationToken);
    }
}