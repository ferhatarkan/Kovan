using Kovan.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize, cancellationToken);

    public static Task<List<TDestination>> ToProjectedListAsync<TDestination>(this IQueryable<TDestination> queryable, CancellationToken cancellationToken = default)
        where TDestination : class
        => queryable.AsNoTracking().ToListAsync(cancellationToken);
}