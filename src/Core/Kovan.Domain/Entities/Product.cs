namespace Kovan.Domain.Entities;
// Domain/Entities/Product.cs
using Kovan.Domain.Enums;
using Kovan.Domain.Common; // BaseEntity'nin namespace'i

public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty; // Stock Keeping Unit - Ürün Kodu
    public decimal Price { get; private set; }
    public Guid CategoryId { get; private set; } // Yeni eklenen kategori ID'si
    public Category? Category { get; private set; } // Navigasyon özelliği
    public string Brand { get; private set; } = string.Empty;

    private readonly List<ProductWarehouse> _productWarehouses = new();
    public IReadOnlyCollection<ProductWarehouse> ProductWarehouses => _productWarehouses.AsReadOnly();

    // Ürüne özel dinamik özellikleri (RAM, Renk, Beden vb.) saklamak için.
    // EF Core bunu veritabanında bir JSON kolonuna haritalayacaktır.
    public Dictionary<string, string> Properties { get; private set; } = new();

    // Private constructor, nesne oluşturmayı kontrollü hale getirir.
    private Product() { }

    // Factory Method: Nesneyi her zaman geçerli bir durumda oluşturmayı sağlar.
    public static Product Create(string name, string sku, decimal price, string brand, Guid categoryId, Dictionary<string, string>? properties = null)
    {
        // İş Kuralları (Validasyonlar)
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ürün adı boş olamaz.");
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("Ürün kodu (SKU) boş olamaz.");
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Marka adı boş olamaz.");
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Kategori ID'si boş olamaz.");
        if (price < 0)
            throw new ArgumentException("Fiyat negatif olamaz.");

        return new Product
        {
            Name = name,
            Sku = sku,
            Price = price,
            CategoryId = categoryId,
            Brand = brand,
            Properties = properties ?? new()
        };
    }

    // Business Logic Method: Stok güncelleme gibi iş kuralları burada yaşar.
    // Bu metot artık ProductWarehouse entity'si üzerinden yönetilmelidir.
    // Product entity'si doğrudan stok miktarını tutmaz.
    public InventoryTransaction CreateInventoryTransaction(Guid warehouseId, int quantity, InventoryTransactionType type, Guid? referenceId = null)
    {
        // Not: Bu metot sadece InventoryTransaction oluşturur.
        // Gerçek stok güncellemesi ProductWarehouse.AdjustStock() metodu ile yapılmalıdır.
        // Bu, bir domain service veya application service tarafından koordine edilmelidir.
        // Örneğin:
        // var productWarehouse = _context.ProductWarehouses.FirstOrDefault(pw => pw.ProductId == this.Id && pw.WarehouseId == warehouseId);
        // productWarehouse.AdjustStock(quantity);
        return InventoryTransaction.Create(this.Id, warehouseId, quantity, type, referenceId);
    }

    // Ürünün temel bilgilerini (meta-data) güncellemek için.
    public void UpdateDetails(string name, string sku, string brand, Guid categoryId, Dictionary<string, string> properties)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ürün adı boş olamaz.");
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("Ürün kodu (SKU) boş olamaz.");
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Marka adı boş olamaz.");
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Kategori ID'si boş olamaz.");

        Name = name;
        Sku = sku;
        CategoryId = categoryId;
        Brand = brand;
        Properties = properties ?? new();
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Yeni fiyat negatif olamaz.");

        Price = newPrice;
    }

    public void SetProperty(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Özellik anahtarı boş olamaz.");

        Properties[key] = value;
    }
}
