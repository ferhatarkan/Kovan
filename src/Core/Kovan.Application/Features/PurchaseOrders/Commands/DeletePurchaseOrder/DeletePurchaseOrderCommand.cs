using MediatR;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder;

public class DeletePurchaseOrderCommand : IRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
}