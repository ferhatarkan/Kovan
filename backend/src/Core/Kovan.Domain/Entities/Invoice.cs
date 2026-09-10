using Kovan.Domain.Common;
using Kovan.Domain.Enums;

namespace Kovan.Domain.Entities;

public class Invoice : BaseEntity // Zaten BaseEntity'den kalıtım alıyor.
{
    public string InvoiceNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public Customer Customer { get; private set; } = null!; // Nullable olmamalı, her faturanın bir müşterisi olmalı.
    public DateTime IssueDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; }

    // Bu alanlar veritabanında tutulmak yerine, ilişkili entity'lerden hesaplanmalı.
    // Bu, veri tutarlılığını sağlar.
    public decimal TotalAmount => InvoiceLines.Sum(l => l.NetTotal);
    public decimal TotalVatAmount => InvoiceLines.Sum(l => l.VatAmount);
    public decimal GrandTotal => InvoiceLines.Sum(l => l.GrossTotal);
    public decimal AmountPaid => Payments.Sum(p => p.Amount);
    public decimal AmountDue => GrandTotal - AmountPaid;

    // Koleksiyonlar için private field ve IReadOnlyCollection property'si doğru bir yaklaşımdır.
    private readonly List<InvoiceLine> _invoiceLines = new();
    public IReadOnlyCollection<InvoiceLine> InvoiceLines => _invoiceLines.AsReadOnly();
    private readonly List<Payment> _payments = new();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    private Invoice() { } // EF Core için private constructor.

    public static Invoice Create(Guid customerId, Guid warehouseId, string invoiceNumber, DateTime dueDate)
    {
        if (customerId == Guid.Empty) throw new ArgumentException("Customer is required.");
        if (warehouseId == Guid.Empty) throw new ArgumentException("Warehouse is required.");
        if (string.IsNullOrWhiteSpace(invoiceNumber)) throw new ArgumentException("Invoice number is required.");

        return new Invoice
        {
            // Id, BaseEntity'den geliyor ve SaveChangesAsync içinde atanacak.
            CustomerId = customerId,
            WarehouseId = warehouseId,
            InvoiceNumber = invoiceNumber,
            IssueDate = DateTime.UtcNow,
            DueDate = dueDate,
            Status = InvoiceStatus.Draft
        };
    }

    public void AddLine(Guid productId, int quantity, decimal unitPrice, decimal vatRate)
    {
        var line = new InvoiceLine(this.Id, productId, quantity, unitPrice, vatRate);
        _invoiceLines.Add(line);
    }

    public void AddPayment(decimal amount, PaymentMethod paymentMethod, DateTime paymentDate, string? notes)
    {
        var payment = Payment.Create(this.Id, amount, paymentMethod, paymentDate, notes);
        _payments.Add(payment);
        UpdateStatus();
    }

    public void Update(string invoiceNumber, DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(invoiceNumber)) throw new ArgumentException("Invoice number is required.");
        InvoiceNumber = invoiceNumber;
        DueDate = dueDate;
    }

    public void ClearLines()
    {
        _invoiceLines.Clear();
    }

    public void UpdateStatusBasedOnPayment()
    {
        UpdateStatus();
    }
    private void UpdateStatus()
    {
        if (AmountPaid >= GrandTotal)
        {
            Status = InvoiceStatus.Paid;
            return;
        }

        if (DateTime.UtcNow > DueDate)
        {
            Status = InvoiceStatus.Overdue;
            return;
        }

        if (AmountPaid > 0 && AmountPaid < GrandTotal)
            Status = InvoiceStatus.PartiallyPaid;
    }
}
