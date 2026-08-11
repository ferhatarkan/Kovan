using MediatR;

namespace Kovan.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder;

public class DeletePurchaseOrderCommand : IRequest
{
    public Guid Id { get; set; }
}