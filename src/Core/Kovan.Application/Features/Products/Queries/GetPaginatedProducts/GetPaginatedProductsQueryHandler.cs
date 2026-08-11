using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Products.Queries.GetPaginatedProducts;

public class GetPaginatedProductsQueryHandler : IRequestHandler<GetPaginatedProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetPaginatedProductsQuery request, CancellationToken cancellationToken)
    {
        return await PaginatedList<ProductDto>.CreateAsync(
            _context.Products
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}