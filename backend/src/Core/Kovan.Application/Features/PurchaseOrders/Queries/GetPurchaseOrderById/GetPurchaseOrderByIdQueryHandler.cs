using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdQueryHandler : IRequestHandler<GetPurchaseOrderByIdQuery, GetPurchaseOrderByIdResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPurchaseOrderByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetPurchaseOrderByIdResult> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _context.PurchaseOrders.AsNoTracking()
            .Where(p => p.Id == request.Id)
            .ProjectTo<GetPurchaseOrderByIdResult>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return purchaseOrder ?? throw new NotFoundException(nameof(PurchaseOrder), request.Id);
    }
}