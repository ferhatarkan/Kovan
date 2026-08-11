using MediatR;
using System.Collections.Generic;

namespace Kovan.Application.Features.Customers.Queries;

public class GetAllCustomersQuery : IRequest<List<CustomerDto>> { }
