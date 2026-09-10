using Kovan.Domain.Common;

namespace Kovan.Domain.Entities;

public class InvoiceLine : BaseEntity
{
    public Guid InvoiceId { get; private set; }
    public Invoice? Invoice { get; private set; }
    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal VatRate { get; private set; }

    // KDV Hariç Tutar (Ara Toplam)
    public decimal NetTotal => Quantity * UnitPrice;
    // Bu satır için hesaplanan KDV Tutarı
    public decimal VatAmount => NetTotal * (VatRate / 100);
    // KDV Dahil Toplam Tutar
    public decimal GrossTotal => NetTotal + VatAmount;

    private InvoiceLine() { } // For EF Core

    public InvoiceLine(Guid invoiceId, Guid productId, int quantity, decimal unitPrice, decimal vatRate)
    {
        InvoiceId = invoiceId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        VatRate = vatRate;
    }
}