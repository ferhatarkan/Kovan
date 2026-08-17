using MediatR;
using Kovan.Application.Common.Interfaces;
using System.Collections.Generic;

namespace Kovan.Application.Features.PurchaseOrders.Commands.UpdatePurchaseOrder;

public class UpdatePurchaseOrderCommand : IRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    // Güncellenecek diğer alanlar buraya eklenebilir, örneğin OrderDate.
}