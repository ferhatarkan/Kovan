using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using MediatR;
using Kovan.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Products.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == request.Id)
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        return product;
    }
}
