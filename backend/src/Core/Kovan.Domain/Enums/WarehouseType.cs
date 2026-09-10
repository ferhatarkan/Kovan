namespace Kovan.Domain.Enums;

public enum WarehouseType
{
    Main = 0,       // Ana Depo
    Display = 1,    // Teşhir Deposu
    Store = 2,      // Mağaza Deposu (Başka bir mağazanın deposu)
    Transit = 3,    // Transit Depo (Ürünlerin taşınırken geçici olarak tutulduğu yer)
    Other = 4       // Diğer
}