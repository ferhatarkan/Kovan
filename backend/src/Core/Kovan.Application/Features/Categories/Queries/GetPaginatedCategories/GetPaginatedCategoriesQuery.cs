using Kovan.Application.Common.Models;
using MediatR;

namespace Kovan.Application.Features.Categories.Queries.GetPaginatedCategories;

public class GetPaginatedCategoriesQuery : IRequest<PaginatedList<GetPaginatedCategoriesResult>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}