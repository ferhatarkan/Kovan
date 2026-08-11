using AutoMapper;
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, SupplierDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSupplierByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SupplierDto> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await _context.Suppliers
                                     .FirstOrDefaultAsync(s => s.Id == request.Id && !s.IsDeleted, cancellationToken)
                                     ?? throw new NotFoundException(nameof(Supplier), request.Id);

        return _mapper.Map<SupplierDto>(supplier);
    }
}