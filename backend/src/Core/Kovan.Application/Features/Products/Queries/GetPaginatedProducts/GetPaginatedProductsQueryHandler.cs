using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Products.Queries.GetPaginatedProducts;

public class GetPaginatedProductsQueryHandler : IRequestHandler<GetPaginatedProductsQuery, PaginatedList<GetPaginatedProductsResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedProductsResult>> Handle(GetPaginatedProductsQuery request, CancellationToken cancellationToken)
    {
        return await PaginatedList<GetPaginatedProductsResult>.CreateAsync(
            _context.Products.Include(p => p.Category) // Category bilgisini dahil et
                .ProjectTo<GetPaginatedProductsResult>(_mapper.ConfigurationProvider)
                .AsNoTracking(),
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}