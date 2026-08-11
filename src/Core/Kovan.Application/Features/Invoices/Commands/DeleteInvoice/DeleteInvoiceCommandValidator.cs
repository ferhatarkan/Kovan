using FluentValidation;

namespace Kovan.Application.Features.Invoices.Commands.DeleteInvoice;

public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Fatura ID'si boş olamaz.");
    }
}