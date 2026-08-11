using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;

namespace Kovan.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        entity.UpdateName(request.Name);
        entity.UpdateSku(request.Sku);
        entity.UpdatePrice(request.Price);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
