using FluentValidation;

namespace Kovan.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Tedarikçi adı zorunludur.")
            .MaximumLength(200).WithMessage("Tedarikçi adı 200 karakterden uzun olamaz.");

        RuleFor(v => v.Email)
            .EmailAddress().When(v => !string.IsNullOrEmpty(v.Email)).WithMessage("Geçerli bir e-posta adresi giriniz.");
    }
}