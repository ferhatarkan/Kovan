using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Kovan.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<GetAllProductsResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<GetAllProductsResult>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .AsNoTracking() // Bu bir okuma işlemi olduğu için değişiklik takibini kapatarak performansı artırırız.
            .Include(p => p.Category) // Category bilgisini dahil et
            .ProjectTo<GetAllProductsResult>(_mapper.ConfigurationProvider) // AutoMapper dönüşümü
            .ToListAsync(cancellationToken);

        return products;
    }
}
