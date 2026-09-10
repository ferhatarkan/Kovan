using MediatR;

namespace Kovan.Application.Features.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdQuery : IRequest<GetSupplierByIdResult>
{
    public Guid Id { get; set; }
}