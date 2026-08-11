namespace Kovan.Domain.Enums;

public enum InventoryTransactionType
{
    Purchase,       // Mal alımı, stok artışı
    Sale,           // Satış, stok azalışı
    Return,         // Müşteri iadesi, stok artışı
    Adjustment,     // Stok sayımı düzeltmesi, artış veya azalış olabilir
    InitialStock    // Başlangıç stoku girişi
}