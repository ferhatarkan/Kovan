using MediatR;
using System.Collections.Generic;

namespace Kovan.Application.Features.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQuery : IRequest<List<GetAllCustomersResult>> { }
