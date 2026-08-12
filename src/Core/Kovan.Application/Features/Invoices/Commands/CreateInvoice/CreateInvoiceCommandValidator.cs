using FluentValidation;

namespace Kovan.Application.Features.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(v => v.CustomerId).NotEmpty();
        RuleFor(v => v.WarehouseId).NotEmpty();
        RuleFor(v => v.InvoiceNumber).NotEmpty().MaximumLength(50);
        RuleFor(v => v.Lines).NotEmpty().WithMessage("Fatura en az bir satır içermelidir.");

        RuleForEach(v => v.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductId).NotEmpty();
            line.RuleFor(l => l.Quantity).GreaterThan(0);
            line.RuleFor(l => l.UnitPrice).GreaterThan(0).WithMessage("Birim fiyat 0'dan büyük olmalıdır.");
            line.RuleFor(l => l.VatRate).InclusiveBetween(0, 100).WithMessage("KDV oranı 0 ile 100 arasında olmalıdır.");
        });
    }
}
