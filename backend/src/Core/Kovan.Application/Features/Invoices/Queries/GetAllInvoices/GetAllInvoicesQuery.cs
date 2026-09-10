using MediatR;
using System.Collections.Generic;

namespace Kovan.Application.Features.Invoices.Queries.GetAllInvoices;

public class GetAllInvoicesQuery : IRequest<List<GetAllInvoicesResult>>
{
}