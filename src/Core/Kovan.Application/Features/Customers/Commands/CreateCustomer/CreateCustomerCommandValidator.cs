using FluentValidation;
using Kovan.Domain.Enums;

namespace Kovan.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(v => v.CustomerType).IsInEnum().WithMessage("Geçerli bir müşteri tipi belirtilmelidir.");

        // Bireysel müşteri için kurallar
        When(v => v.CustomerType == CustomerType.Individual, () =>
        {
            RuleFor(v => v.FirstName).NotEmpty().WithMessage("İsim alanı zorunludur.");
            RuleFor(v => v.LastName).NotEmpty().WithMessage("Soyisim alanı zorunludur.");
            RuleFor(v => v.NationalIdentityNumber).NotEmpty().Length(11).WithMessage("T.C. Kimlik Numarası 11 haneli olmalıdır.");
        });

        // Kurumsal müşteri için kurallar
        When(v => v.CustomerType == CustomerType.Corporate, () =>
        {
            RuleFor(v => v.Title).NotEmpty().WithMessage("Ünvan alanı zorunludur.");
            RuleFor(v => v.TaxNumber).NotEmpty().Length(10).WithMessage("Vergi Numarası 10 haneli olmalıdır.");
        });
    }
}