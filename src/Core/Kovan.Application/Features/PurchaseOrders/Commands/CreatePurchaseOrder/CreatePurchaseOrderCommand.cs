using MediatR;
using System.Collections.Generic;

namespace Kovan.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderCommand : IRequest<Guid>
{
    public Guid SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<CreatePurchaseOrderLineDto> Lines { get; set; } = new();
}