using AutoMapper;
using Kovan.Application.Features.Categories.Queries.GetAllCategories;
using Kovan.Application.Features.Categories.Queries.GetCategoryById;
using Kovan.Application.Features.Customers.Queries.GetAllCustomers;
using Kovan.Application.Features.Customers.Queries.GetCustomerById;
using Kovan.Application.Features.Categories.Queries.GetPaginatedCategories;
using Kovan.Application.Features.Customers.Queries.GetPaginatedCustomers;
using Kovan.Application.Features.Invoices.Queries.GetAllInvoices;
using Kovan.Application.Features.Invoices.Queries.GetInvoiceById;
using Kovan.Application.Features.Invoices.Queries.GetPaginatedInvoices;
using Kovan.Application.Features.Products.Queries.GetAllProducts;
using Kovan.Application.Features.Products.Queries.GetProductById;
using Kovan.Application.Features.Products.Queries.GetPaginatedProducts;
using Kovan.Application.Features.Suppliers.Queries.GetPaginatedSuppliers;
using Kovan.Application.Features.Suppliers.Queries.GetSupplierById;
using Kovan.Application.Features.Suppliers.Queries.GetSuppliers;
using Kovan.Application.Features.PurchaseOrders.Queries.GetPaginatedPurchaseOrders;
using Kovan.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;
using Kovan.Application.Features.Tenants.Queries.GetTenantSettings;
using Kovan.Domain.Entities;

namespace Kovan.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Category Mappings
        CreateMap<Category, GetAllCategoriesResult>()
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null));
        CreateMap<Category, GetCategoryByIdResult>()
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null));
        CreateMap<Category, GetPaginatedCategoriesResult>()
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null));


        // Product Mappings
        CreateMap<Product, GetAllProductsResult>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
        CreateMap<Product, GetProductByIdResult>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
        CreateMap<Product, GetPaginatedProductsResult>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

        // Customer Mappings
        CreateMap<Customer, GetCustomerByIdResult>();

        CreateMap<Customer, GetAllCustomersResult>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src =>
                src.CustomerType == Kovan.Domain.Enums.CustomerType.Individual ? $"{src.FirstName} {src.LastName}" : src.Title));
        CreateMap<Customer, GetPaginatedCustomersResult>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src =>
                src.CustomerType == Kovan.Domain.Enums.CustomerType.Individual ? $"{src.FirstName} {src.LastName}" : src.Title));

        // Invoice Mappings
        CreateMap<Invoice, GetPaginatedInvoicesResult>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? (src.Customer.Title ?? $"{src.Customer.FirstName} {src.Customer.LastName}") : string.Empty));

        CreateMap<Invoice, GetAllInvoicesResult>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? (src.Customer.Title ?? $"{src.Customer.FirstName} {src.Customer.LastName}") : string.Empty));

        CreateMap<Invoice, GetInvoiceByIdResult>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? (src.Customer.Title ?? $"{src.Customer.FirstName} {src.Customer.LastName}") : string.Empty));

        CreateMap<InvoiceLine, InvoiceLineItem>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));

        // Supplier Mappings
        CreateMap<Supplier, GetSuppliersResult>();
        CreateMap<Supplier, GetSupplierByIdResult>();
        CreateMap<Supplier, GetPaginatedSuppliersResult>();

        // PurchaseOrder Mappings
        CreateMap<PurchaseOrder, GetPaginatedPurchaseOrdersResult>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty));
        CreateMap<PurchaseOrder, GetPurchaseOrderByIdResult>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty));
        CreateMap<PurchaseOrderLine, PurchaseOrderLineItem>();

        // Tenant Mappings
        CreateMap<Tenant, GetTenantSettingsResult>();
    }
}
