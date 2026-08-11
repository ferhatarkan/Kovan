using MediatR;

namespace Kovan.Application.Features.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdQuery : IRequest<SupplierDto>
{
    public Guid Id { get; set; }
}