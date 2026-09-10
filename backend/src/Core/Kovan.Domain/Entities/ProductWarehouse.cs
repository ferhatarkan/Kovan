using Kovan.Domain.Common;

namespace Kovan.Domain.Entities;

public class ProductWarehouse : BaseEntity
{
    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }

    public Guid WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }

    public int StockQuantity { get; private set; }

    private ProductWarehouse() { }

    public static ProductWarehouse Create(Guid productId, Guid warehouseId, int initialStock = 0)
    {
        return new ProductWarehouse
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            StockQuantity = initialStock
        };
    }

    public void AdjustStock(int quantityChange)
    {
        if (StockQuantity + quantityChange < 0)
            throw new InvalidOperationException("Stok miktarı eksiye düşemez.");
        StockQuantity += quantityChange;
    }
}