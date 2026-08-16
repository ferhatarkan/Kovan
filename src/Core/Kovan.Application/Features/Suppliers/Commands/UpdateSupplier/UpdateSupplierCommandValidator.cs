using FluentValidation;

namespace Kovan.Application.Features.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Tedarikçi ID'si boş olamaz.");
        RuleFor(v => v.Name).NotEmpty().MaximumLength(200).WithMessage("Tedarikçi adı boş olamaz ve 200 karakterden uzun olamaz.");
        RuleFor(v => v.PhoneNumber).NotEmpty().WithMessage("Telefon numarası boş olamaz.");
        RuleFor(v => v.Email).EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.").When(v => !string.IsNullOrEmpty(v.Email));
        RuleFor(v => v.Address).NotEmpty().WithMessage("Adres boş olamaz.");
    }
}