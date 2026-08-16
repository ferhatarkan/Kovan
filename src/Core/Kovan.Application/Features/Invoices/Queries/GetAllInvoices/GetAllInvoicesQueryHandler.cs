using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Invoices.Queries.GetAllInvoices;

public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, List<GetAllInvoicesResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllInvoicesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<GetAllInvoicesResult>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Invoices
            .ProjectTo<GetAllInvoicesResult>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}