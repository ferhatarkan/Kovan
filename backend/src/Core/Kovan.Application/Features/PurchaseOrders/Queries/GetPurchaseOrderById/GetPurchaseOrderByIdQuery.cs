using MediatR;

namespace Kovan.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdQuery : IRequest<GetPurchaseOrderByIdResult>
{
    public Guid Id { get; set; }
}