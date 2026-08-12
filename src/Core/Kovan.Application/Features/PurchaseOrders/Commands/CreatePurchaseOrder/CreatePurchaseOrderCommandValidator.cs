using FluentValidation;

namespace Kovan.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderCommandValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
    public CreatePurchaseOrderCommandValidator()
    {
        RuleFor(v => v.SupplierId)
            .NotEmpty().WithMessage("Tedarikçi ID'si boş olamaz.");

        RuleFor(v => v.OrderDate)
            .NotEmpty().WithMessage("Sipariş tarihi boş olamaz.");

        RuleFor(v => v.Lines)
            .NotEmpty().WithMessage("Sipariş en az bir satır içermelidir.");

        RuleForEach(v => v.Lines).SetValidator(new LineItemDtoValidator());
    }
}

public class LineItemDtoValidator : AbstractValidator<CreatePurchaseOrderCommand.LineItemDto> // Sınıf adı ile constructor adı aynı olmalı
{
    public LineItemDtoValidator() // Constructor adı düzeltildi
    {
        RuleFor(l => l.ProductId).NotEmpty();
        RuleFor(l => l.Quantity).GreaterThan(0);
        RuleFor(l => l.PurchasePrice).GreaterThanOrEqualTo(0);
    }
}