using Kovan.Application.Common.Models;
using Kovan.Application.Features.Products.Queries;
using MediatR;

namespace Kovan.Application.Features.Products.Queries.GetPaginatedProducts;

public class GetPaginatedProductsQuery : IRequest<PaginatedList<ProductDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}