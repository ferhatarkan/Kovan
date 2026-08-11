using Kovan.Domain.Common;

namespace Kovan.Domain.Entities;

public class PurchaseOrderLine : BaseEntity
{
    public Guid PurchaseOrderId { get; private set; }
    public PurchaseOrder? PurchaseOrder { get; private set; }
    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }
    public int Quantity { get; private set; }
    public decimal PurchasePrice { get; private set; } // KDV Hariç Birim Fiyat
    public decimal VatRate { get; private set; } // KDV Oranı

    // Hesaplanan Alanlar
    public decimal NetTotal => Quantity * PurchasePrice;
    public decimal VatAmount => NetTotal * (VatRate / 100);
    public decimal GrossTotal => NetTotal + VatAmount;

    private PurchaseOrderLine() { }

    public PurchaseOrderLine(Guid purchaseOrderId, Guid productId, int quantity, decimal purchasePrice, decimal vatRate)
    {
        PurchaseOrderId = purchaseOrderId;
        ProductId = productId;
        Quantity = quantity;
        PurchasePrice = purchasePrice;
        VatRate = vatRate;
    }
}