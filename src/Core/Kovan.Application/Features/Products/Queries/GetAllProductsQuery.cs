using MediatR;

namespace Kovan.Application.Features.Products.Queries;

public class GetAllProductsQuery : IRequest<List<ProductDto>>
{
}

