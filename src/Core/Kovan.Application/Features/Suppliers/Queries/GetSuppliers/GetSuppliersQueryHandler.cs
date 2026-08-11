using AutoMapper;
using Kovan.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, List<SupplierDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSuppliersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SupplierDto>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _context.Suppliers
                                      .Where(s => !s.IsDeleted) // Sadece silinmemiş tedarikçileri getir
                                      .OrderBy(s => s.Name)
                                      .ToListAsync(cancellationToken);

        return _mapper.Map<List<SupplierDto>>(suppliers);
    }
}