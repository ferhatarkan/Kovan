using MediatR;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommand : IRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
}