using MediatR;

namespace Kovan.Application.Features.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQuery : IRequest<List<SupplierDto>>
{
}