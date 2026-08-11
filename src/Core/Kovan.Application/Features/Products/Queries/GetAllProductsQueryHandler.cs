using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Products.Queries;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .AsNoTracking() // Bu bir okuma işlemi olduğu için değişiklik takibini kapatarak performansı artırırız.
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider) // AutoMapper dönüşümü
            .ToListAsync(cancellationToken);

        return products;
    }
}
