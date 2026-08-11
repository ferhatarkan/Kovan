using Kovan.Domain.Enums;

namespace Kovan.Application.Features.PurchaseOrders.Dtos;

public class PurchaseOrderDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public PurchaseOrderStatus Status { get; set; }
    public string StatusAsString => Status.ToString();
}