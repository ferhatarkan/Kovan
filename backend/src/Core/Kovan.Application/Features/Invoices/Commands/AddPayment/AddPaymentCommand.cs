using Kovan.Domain.Enums;
using MediatR;
using Kovan.Application.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Kovan.Application.Features.Invoices.Commands.AddPayment;

public class AddPaymentCommand : IRequest, ITransactionalRequest
{
    [JsonIgnore] // InvoiceId rotadan (route) alınacak.
    public Guid InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}