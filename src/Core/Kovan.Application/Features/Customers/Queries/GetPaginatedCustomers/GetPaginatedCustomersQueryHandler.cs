using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Customers.Queries.GetPaginatedCustomers;

public class GetPaginatedCustomersQueryHandler : IRequestHandler<GetPaginatedCustomersQuery, PaginatedList<GetPaginatedCustomersResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPaginatedCustomersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedCustomersResult>> Handle(GetPaginatedCustomersQuery request, CancellationToken cancellationToken)
    {
        return await PaginatedList<GetPaginatedCustomersResult>.CreateAsync(
            _context.Customers
                .ProjectTo<GetPaginatedCustomersResult>(_mapper.ConfigurationProvider)
                .AsNoTracking(),
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
