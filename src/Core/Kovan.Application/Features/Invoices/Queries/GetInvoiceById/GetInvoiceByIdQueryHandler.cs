using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Features.Invoices.Queries.GetInvoiceById;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Invoices.Queries.GetInvoiceById;

public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, GetInvoiceByIdResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInvoiceByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetInvoiceByIdResult> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoiceResult = await _context.Invoices
            .Where(i => i.Id == request.Id)
            .ProjectTo<GetInvoiceByIdResult>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (invoiceResult == null)
        {
            throw new NotFoundException(nameof(Invoice), request.Id);
        }

        return invoiceResult;
    }
}
