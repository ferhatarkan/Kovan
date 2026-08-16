using MediatR;

namespace Kovan.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQuery : IRequest<GetCustomerByIdResult>
{
    public Guid Id { get; set; }
}