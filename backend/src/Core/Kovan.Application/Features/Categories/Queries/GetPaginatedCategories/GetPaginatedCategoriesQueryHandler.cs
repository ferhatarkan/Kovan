using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Categories.Queries.GetPaginatedCategories;

public class GetPaginatedCategoriesQueryHandler : IRequestHandler<GetPaginatedCategoriesQuery, PaginatedList<GetPaginatedCategoriesResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedCategoriesResult>> Handle(GetPaginatedCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await PaginatedList<GetPaginatedCategoriesResult>.CreateAsync(
            _context.Categories.AsNoTracking().Include(c => c.ParentCategory).OrderBy(c => c.Name)
                .ProjectTo<GetPaginatedCategoriesResult>(_mapper.ConfigurationProvider),
            request.PageNumber, request.PageSize, cancellationToken);
    }
}