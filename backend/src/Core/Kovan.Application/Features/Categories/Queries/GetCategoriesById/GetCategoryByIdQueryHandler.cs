using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCategoryByIdResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.AsNoTracking()
            .Where(c => c.Id == request.Id)
            .ProjectTo<GetCategoryByIdResult>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return category ?? throw new NotFoundException(nameof(Category), request.Id);
    }
}