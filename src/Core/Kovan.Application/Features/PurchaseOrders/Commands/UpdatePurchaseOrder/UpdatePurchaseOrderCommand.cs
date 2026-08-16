using MediatR;
using System.Collections.Generic;

namespace Kovan.Application.Features.PurchaseOrders.Commands.UpdatePurchaseOrder;

public class UpdatePurchaseOrderCommand : IRequest
{
    public Guid Id { get; set; }
    // Güncellenecek diğer alanlar buraya eklenebilir, örneğin OrderDate.
}