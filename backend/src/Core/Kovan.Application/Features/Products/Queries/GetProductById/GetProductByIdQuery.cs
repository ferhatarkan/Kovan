using MediatR;

namespace Kovan.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<GetProductByIdResult>
{
    public Guid Id { get; set; }
}