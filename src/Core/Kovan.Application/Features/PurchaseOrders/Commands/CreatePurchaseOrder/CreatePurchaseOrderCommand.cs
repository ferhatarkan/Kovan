using Kovan.Application.Common.Interfaces;
using MediatR;

namespace Kovan.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderCommand : IRequest<Guid>, ITransactionalRequest
{
    public Guid SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public List<LineItemDto> Lines { get; set; } = new();

    public class LineItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal VatRate { get; set; }
    }
}