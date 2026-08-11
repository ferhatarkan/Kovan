using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Invoices.Queries;

public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInvoiceByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoiceDto = await _context.Invoices
            .Where(i => i.Id == request.Id)
            .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (invoiceDto == null)
        {
            throw new NotFoundException(nameof(Invoice), request.Id);
        }

        return invoiceDto;
    }
}
