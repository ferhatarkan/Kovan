using MediatR;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Features.Suppliers.Commands.DeleteSupplier;

public class DeleteSupplierCommand : IRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
}