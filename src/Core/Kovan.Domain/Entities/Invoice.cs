using Kovan.Domain.Common;
using Kovan.Domain.Enums;

namespace Kovan.Domain.Entities;

public class Invoice : BaseEntity
{
    public string InvoiceNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public Customer? Customer { get; private set; }
    public DateTime IssueDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; }

    public decimal TotalAmount => InvoiceLines.Sum(l => l.NetTotal);
    public decimal TotalVatAmount => InvoiceLines.Sum(l => l.VatAmount);
    public decimal GrandTotal => InvoiceLines.Sum(l => l.GrossTotal);
    public decimal AmountPaid => Payments.Sum(p => p.Amount);
    public decimal AmountDue => GrandTotal - AmountPaid;

    private readonly List<InvoiceLine> _invoiceLines = new();
    public IReadOnlyCollection<InvoiceLine> InvoiceLines => _invoiceLines.AsReadOnly();
    private readonly List<Payment> _payments = new();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    private Invoice() { } // For EF Core

    public static Invoice Create(Guid customerId, string invoiceNumber, DateTime dueDate)
    {
        // Basic validation
        if (customerId == Guid.Empty) throw new ArgumentException("Customer is required.");
        if (string.IsNullOrWhiteSpace(invoiceNumber)) throw new ArgumentException("Invoice number is required.");

        return new Invoice
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            InvoiceNumber = invoiceNumber,
            IssueDate = DateTime.UtcNow,
            DueDate = dueDate,
            Status = InvoiceStatus.Draft // Fatura ilk oluşturulduğunda taslak durumundadır.
        };
    }

    public void AddLine(Guid productId, int quantity, decimal unitPrice, decimal vatRate)
    {
        // TODO: Stok kontrolü gibi daha karmaşık iş kuralları buraya eklenebilir.
        var line = new InvoiceLine(this.Id, productId, quantity, unitPrice, vatRate);
        _invoiceLines.Add(line);
    }

    public void AddPayment(decimal amount, PaymentMethod paymentMethod, DateTime paymentDate, string? notes)
    {
        var payment = Payment.Create(this.Id, amount, paymentMethod, paymentDate, notes);
        _payments.Add(payment);
        UpdateStatus(); // Bu metot zaten ödeme durumuna göre status'u güncelliyor.
    }

    public void Update(string invoiceNumber, DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(invoiceNumber)) throw new ArgumentException("Invoice number is required.");
        InvoiceNumber = invoiceNumber;
        DueDate = dueDate;
    }

    public void ClearLines()
    {
        // Bu, mevcut tüm satırları siler. Daha karmaşık senaryolarda,
        // satırları tek tek güncellemek veya silmek için farklı metotlar yazılabilir.
        _invoiceLines.Clear();
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void UpdateStatusBasedOnPayment()
    {
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        // If it's already paid, no other status matters.
        if (AmountPaid >= GrandTotal)
        {
            Status = InvoiceStatus.Paid;
            return;
        }

        // If it's not fully paid, check if it's overdue. This has higher priority.
        if (DateTime.UtcNow > DueDate)
        {
            Status = InvoiceStatus.Overdue; // 'Paid' kontrolü zaten yukarıda yapıldığı için buradaki kontrol gereksizdir.
            return;
        }

        // If not paid and not overdue, check if it's partially paid.
        // Bu bloğu daha açık hale getirmek için if-else yapısı kullanalım.
        else if (AmountPaid > 0 && AmountPaid < GrandTotal)
        {
            Status = InvoiceStatus.PartiallyPaid;
        }
        // Hiç ödeme yapılmadıysa ve vadesi geçmediyse, durumu 'Gönderildi' olarak varsayabiliriz.
        // (Eğer 'Draft'tan sonra 'Sent' gibi bir durum varsa)
        else
        {
            // Eğer fatura ilk oluşturulduğunda Draft ise ve hiç ödeme yoksa,
            // durumunu değiştirmemek en güvenlisi olabilir.
            // Veya iş akışınıza göre burası 'Sent' veya 'Draft' olarak ayarlanabilir.
            // Şimdilik mevcut davranışı korumak adına bu else bloğu boş bırakılabilir
            // veya başlangıç durumuna (örn: Draft) geri döndürülebilir.
            // Örneğin: Status = InvoiceStatus.Draft;
        }
    }
}