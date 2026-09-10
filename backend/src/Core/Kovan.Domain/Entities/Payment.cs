using Kovan.Domain.Common;
using Kovan.Domain.Enums;

namespace Kovan.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid InvoiceId { get; private set; }
    public Invoice? Invoice { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public string? Notes { get; private set; }

    private Payment() { } // For EF Core

    public static Payment Create(Guid invoiceId, decimal amount, PaymentMethod paymentMethod, DateTime paymentDate, string? notes)
    {
        if (amount <= 0) throw new ArgumentException("Ödeme tutarı pozitif olmalıdır.");

        return new Payment
        {
            InvoiceId = invoiceId,
            Amount = amount,
            PaymentMethod = paymentMethod,
            PaymentDate = paymentDate,
            Notes = notes
        };
    }
}