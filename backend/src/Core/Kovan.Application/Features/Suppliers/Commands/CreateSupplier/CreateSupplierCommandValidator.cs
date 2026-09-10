using FluentValidation;

namespace Kovan.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200).WithMessage("Tedarikçi adı boş olamaz ve 200 karakterden uzun olamaz.");
        RuleFor(v => v.PhoneNumber).NotEmpty().WithMessage("Telefon numarası boş olamaz.");
        RuleFor(v => v.Email).EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.").When(v => !string.IsNullOrEmpty(v.Email));
        RuleFor(v => v.Address).NotEmpty().WithMessage("Adres boş olamaz.");
    }
}