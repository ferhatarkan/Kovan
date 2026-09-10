using Kovan.Domain.Enums;

namespace Kovan.Application.Features.PurchaseOrders.Queries.GetPaginatedPurchaseOrders;

public class GetPaginatedPurchaseOrdersResult
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public PurchaseOrderStatus Status { get; set; }
}