using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Models;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Invoices.Queries.GetPaginatedInvoices;

public class GetPaginatedInvoicesQueryHandler : IRequestHandler<GetPaginatedInvoicesQuery, PaginatedList<GetPaginatedInvoicesResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedInvoicesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedInvoicesResult>> Handle(GetPaginatedInvoicesQuery request, CancellationToken cancellationToken)
    {
        return await PaginatedList<GetPaginatedInvoicesResult>.CreateAsync(
            _context.Invoices
                .ProjectTo<GetPaginatedInvoicesResult>(_mapper.ConfigurationProvider)
                .AsNoTracking(), // Sadece okuma amaçlı olduğu için izlemeyi kapat
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
