using Kovan.Domain.Common;
using Kovan.Domain.Enums;

namespace Kovan.Domain.Entities;

public class InventoryTransaction : BaseEntity
{
    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }
    public Guid WarehouseId { get; private set; } // Hangi depoda gerçekleştiği
    public Warehouse? Warehouse { get; private set; }
    public int QuantityChanged { get; private set; } // Pozitif: Stok girişi, Negatif: Stok çıkışı
    // public int ResultingStock { get; private set; } // Artık ProductWarehouse'da tutuluyor
    public InventoryTransactionType TransactionType { get; private set; }
    public Guid? ReferenceId { get; private set; } // İlişkili Fatura ID'si, Alım ID'si vb.

    private InventoryTransaction() { }

    public static InventoryTransaction Create(Guid productId, Guid warehouseId, int quantityChanged, InventoryTransactionType type, Guid? referenceId = null)
    {
        return new InventoryTransaction { ProductId = productId, WarehouseId = warehouseId, QuantityChanged = quantityChanged, TransactionType = type, ReferenceId = referenceId };
    }
}