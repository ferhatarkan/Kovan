using Kovan.Domain.Common;
using Kovan.Domain.Enums;

namespace Kovan.Domain.Entities;

public class PurchaseOrder : BaseEntity
{
    public string OrderNumber { get; private set; } = string.Empty;
    public Guid SupplierId { get; private set; }
    public Supplier? Supplier { get; private set; }
    public DateTime OrderDate { get; private set; }
    public PurchaseOrderStatus Status { get; private set; }

    // Hesaplanan Toplamlar
    public decimal TotalAmount => Lines.Sum(l => l.NetTotal);
    public decimal TotalVatAmount => Lines.Sum(l => l.VatAmount);
    public decimal GrandTotal => Lines.Sum(l => l.GrossTotal);

    private readonly List<PurchaseOrderLine> _lines = new();
    public IReadOnlyCollection<PurchaseOrderLine> Lines => _lines.AsReadOnly();

    private PurchaseOrder() { }

    public static PurchaseOrder Create(Guid supplierId, DateTime orderDate, string orderNumber)
    {
        return new PurchaseOrder
        {
            SupplierId = supplierId,
            OrderDate = orderDate,
            OrderNumber = orderNumber,
            Status = PurchaseOrderStatus.Draft
        };
    }

    public void AddLine(Guid productId, int quantity, decimal purchasePrice, decimal vatRate)
    {
        var line = new PurchaseOrderLine(this.Id, productId, quantity, purchasePrice, vatRate);
        _lines.Add(line);
    }

    public void Delete() => IsDeleted = true;
}