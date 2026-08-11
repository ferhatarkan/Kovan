using MediatR;

namespace Kovan.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest
{
    public Guid Id { get; set; }
}
