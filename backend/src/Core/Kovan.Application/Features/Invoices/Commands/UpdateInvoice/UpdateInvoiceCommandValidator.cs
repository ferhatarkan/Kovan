using FluentValidation;

namespace Kovan.Application.Features.Invoices.Commands.UpdateInvoice;

public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
{
    public UpdateInvoiceCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Fatura ID'si boş olamaz.");
        RuleFor(v => v.InvoiceNumber).NotEmpty().WithMessage("Fatura numarası boş olamaz.");
    }
}