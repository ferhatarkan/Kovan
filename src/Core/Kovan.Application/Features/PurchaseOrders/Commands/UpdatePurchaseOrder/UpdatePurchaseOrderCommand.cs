using MediatR;

namespace Kovan.Application.Features.PurchaseOrders.Commands.UpdatePurchaseOrder;

public class UpdatePurchaseOrderCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public List<UpdatePurchaseOrderLineItem> Lines { get; set; } = new();
}

public class UpdatePurchaseOrderLineItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal VatRate { get; set; }
}
