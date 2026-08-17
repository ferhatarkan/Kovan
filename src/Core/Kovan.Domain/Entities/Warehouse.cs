using Kovan.Domain.Common;
using Kovan.Domain.Enums; // Eğer depo tipleri için enum kullanacaksanız

namespace Kovan.Domain.Entities;

public class Warehouse : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? LocationAddress { get; private set; }
    public WarehouseType Type { get; private set; } // Örneğin: Display, Main, Store

    private readonly List<ProductWarehouse> _productWarehouses = new();
    public IReadOnlyCollection<ProductWarehouse> ProductWarehouses => _productWarehouses.AsReadOnly();

    private Warehouse() { }

    public static Warehouse Create(string name, string? locationAddress, WarehouseType type)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Depo adı boş olamaz.");

        return new Warehouse
        {
            Name = name,
            LocationAddress = locationAddress,
            Type = type
        };
    }

    public void Update(string name, string? locationAddress, WarehouseType type) => (Name, LocationAddress, Type) = (name, locationAddress, type);
}