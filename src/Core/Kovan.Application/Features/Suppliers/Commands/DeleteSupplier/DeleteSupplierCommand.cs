using MediatR;

namespace Kovan.Application.Features.Suppliers.Commands.DeleteSupplier;

public class DeleteSupplierCommand : IRequest
{
    public Guid Id { get; set; }
}