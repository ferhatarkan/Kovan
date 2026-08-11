using MediatR;

namespace Kovan.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommand : IRequest
{
    public Guid Id { get; set; }
}