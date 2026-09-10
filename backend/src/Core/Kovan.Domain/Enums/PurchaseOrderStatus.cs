namespace Kovan.Domain.Enums;

public enum PurchaseOrderStatus
{
    Draft = 0,      // Taslak: Sipariş oluşturuluyor, henüz gönderilmedi.
    Submitted = 1,  // Gönderildi: Sipariş tedarikçiye iletildi.
    Approved = 2,   // Onaylandı: Sipariş onaylandı, mal teslimi bekleniyor.
    Completed = 3,  // Tamamlandı: Mallar teslim alındı ve stoklara işlendi.
    Cancelled = 4   // İptal Edildi: Sipariş iptal edildi.
}