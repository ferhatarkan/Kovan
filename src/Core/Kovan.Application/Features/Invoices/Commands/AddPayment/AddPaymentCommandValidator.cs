using FluentValidation;

namespace Kovan.Application.Features.Invoices.Commands.AddPayment;

public class AddPaymentCommandValidator : AbstractValidator<AddPaymentCommand>
{
    public AddPaymentCommandValidator()
    {
        RuleFor(v => v.InvoiceId).NotEmpty().WithMessage("Fatura ID'si boş olamaz.");
        RuleFor(v => v.Amount).GreaterThan(0).WithMessage("Ödeme tutarı 0'dan büyük olmalıdır.");
    }
}