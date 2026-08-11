using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Invoices.Queries;

public sealed class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, List<InvoiceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllInvoicesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<List<InvoiceDto>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken) =>
        _context.Invoices
            .AsNoTracking()
            .OrderByDescending(invoice => invoice.IssueDate)
            .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
}
