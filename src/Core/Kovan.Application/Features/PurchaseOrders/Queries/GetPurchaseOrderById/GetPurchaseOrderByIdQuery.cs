using Kovan.Application.Features.PurchaseOrders.Dtos;
using MediatR;

namespace Kovan.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdQuery : IRequest<PurchaseOrderDto>
{
    public Guid Id { get; set; }
}
