using MediatR;
using System.Collections.Generic;

namespace Kovan.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQuery : IRequest<List<GetAllProductsResult>>
{
}
