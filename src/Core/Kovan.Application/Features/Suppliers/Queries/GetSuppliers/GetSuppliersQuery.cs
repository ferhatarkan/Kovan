using MediatR;
using System.Collections.Generic;

namespace Kovan.Application.Features.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQuery : IRequest<List<GetSuppliersResult>>
{
}