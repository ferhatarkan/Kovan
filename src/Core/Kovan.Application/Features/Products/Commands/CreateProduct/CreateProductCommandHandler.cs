using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;

namespace Kovan.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(request.Name, request.Sku, request.Price, request.Properties);

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product.Id;
    }
}