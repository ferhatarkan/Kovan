using MediatR;

namespace Kovan.Application.Features.Customers.Queries;

public class GetCustomerByIdQuery : IRequest<CustomerDto>
{
    public Guid Id { get; set; }
}