using MediatR;

namespace Kovan.Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<ProductDto>
{
    public Guid Id { get; set; }
}