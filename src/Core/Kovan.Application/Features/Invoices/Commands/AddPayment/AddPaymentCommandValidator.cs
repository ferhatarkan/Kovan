using FluentValidation;

namespace Kovan.Application.Features.Invoices.Commands.AddPayment;

public class AddPaymentCommandValidator : AbstractValidator<AddPaymentCommand>
{
    public AddPaymentCommandValidator()
    {
        RuleFor(v => v.InvoiceId).NotEmpty();
        RuleFor(v => v.Amount).GreaterThan(0).WithMessage("Ödeme tutarı 0'dan büyük olmalıdır.");
        RuleFor(v => v.PaymentMethod).IsInEnum().WithMessage("Geçerli bir ödeme yöntemi belirtilmelidir.");
        RuleFor(v => v.PaymentDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Ödeme tarihi gelecek bir tarih olamaz.");
    }
}