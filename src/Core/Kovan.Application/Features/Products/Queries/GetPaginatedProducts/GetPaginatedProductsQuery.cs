using Kovan.Application.Common.Models;
using MediatR;

namespace Kovan.Application.Features.Products.Queries.GetPaginatedProducts;

public class GetPaginatedProductsQuery : IRequest<PaginatedList<GetPaginatedProductsResult>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}