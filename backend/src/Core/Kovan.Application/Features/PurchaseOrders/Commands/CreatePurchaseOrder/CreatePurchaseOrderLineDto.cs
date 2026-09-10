namespace Kovan.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderLineDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal VatRate { get; set; } // Added VAT Rate
}