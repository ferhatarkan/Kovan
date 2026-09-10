using System.Reflection;
using FluentValidation;
using Kovan.Application.Features.Categories.Commands.DeleteCategory;
using Kovan.Application.Features.Categories.Commands.UpdateCategory;
using Kovan.Application.Features.Customers.Commands.DeleteCustomer;
using Kovan.Application.Features.Invoices.Commands.CreateInvoice;
using Kovan.Application.Features.Invoices.Commands.DeleteInvoice;
using Kovan.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;
using Kovan.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder;
using Kovan.Application.Features.Suppliers.Commands.DeleteSupplier;
using MediatR;
using Kovan.Application.Common.Behaviours; // 
using Microsoft.Extensions.DependencyInjection;

namespace Kovan.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Otomatik olarak tüm AbstractValidator'ları bulur ve kaydeder.
        // Bu, sadece parametresiz constructor'a sahip olanlar için çalışır.
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // IApplicationDbContext gibi bağımlılıkları olan validator'ları manuel olarak kaydetmeliyiz.
        services.AddScoped<IValidator<DeleteSupplierCommand>, DeleteSupplierCommandValidator>();
        services.AddScoped<IValidator<DeleteCategoryCommand>, DeleteCategoryCommandValidator>();
        services.AddScoped<IValidator<UpdateCategoryCommand>, UpdateCategoryCommandValidator>();
        services.AddScoped<IValidator<CreateInvoiceCommand>, CreateInvoiceCommandValidator>();
        services.AddScoped<IValidator<DeleteInvoiceCommand>, DeleteInvoiceCommandValidator>();
        services.AddScoped<IValidator<DeleteCustomerCommand>, DeleteCustomerCommandValidator>();
        services.AddScoped<IValidator<CreatePurchaseOrderCommand>, CreatePurchaseOrderCommandValidator>();
        services.AddScoped<IValidator<DeletePurchaseOrderCommand>, DeletePurchaseOrderCommandValidator>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
