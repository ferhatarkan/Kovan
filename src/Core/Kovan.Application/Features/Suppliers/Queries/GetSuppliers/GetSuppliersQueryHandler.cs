using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, List<GetSuppliersResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetSuppliersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<GetSuppliersResult>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Suppliers
            .OrderBy(s => s.Name)
            .ProjectTo<GetSuppliersResult>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}