using Kovan.Domain.Entities;
using Xunit;

namespace Kovan.Domain.Tests;

public class ProductWarehouseTests
{
    [Fact]
    public void AdjustStock_Throws_WhenStockWouldBecomeNegative()
    {
        var stock = ProductWarehouse.Create(Guid.NewGuid(), Guid.NewGuid(), initialStock: 2);

        Assert.Throws<InvalidOperationException>(() => stock.AdjustStock(-3));
    }

    [Fact]
    public void AdjustStock_UpdatesAvailableQuantity()
    {
        var stock = ProductWarehouse.Create(Guid.NewGuid(), Guid.NewGuid(), initialStock: 2);

        stock.AdjustStock(-1);

        Assert.Equal(1, stock.StockQuantity);
    }
}
