using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder;

public class DeletePurchaseOrderCommandHandler : IRequestHandler<DeletePurchaseOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePurchaseOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PurchaseOrders.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null) throw new NotFoundException(nameof(PurchaseOrder), request.Id);

        _context.PurchaseOrders.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}