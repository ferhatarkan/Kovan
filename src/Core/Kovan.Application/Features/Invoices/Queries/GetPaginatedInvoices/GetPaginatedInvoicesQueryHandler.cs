using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Models;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Invoices.Queries.GetPaginatedInvoices;

public class GetPaginatedInvoicesQueryHandler : IRequestHandler<GetPaginatedInvoicesQuery, PaginatedList<InvoiceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedInvoicesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<InvoiceDto>> Handle(GetPaginatedInvoicesQuery request, CancellationToken cancellationToken)
    {
        return await PaginatedList<InvoiceDto>.CreateAsync(
            _context.Invoices
                .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(), // Sadece okuma amaçlı olduğu için izlemeyi kapat
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
