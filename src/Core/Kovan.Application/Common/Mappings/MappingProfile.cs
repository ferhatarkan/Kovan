using AutoMapper;
using Kovan.Application.Features.Products.Queries;
using Kovan.Application.Features.Invoices.Queries;
using Kovan.Application.Features.Customers.Queries;
using Kovan.Application.Features.Suppliers.Queries;
using Kovan.Application.Features.PurchaseOrders.Dtos;
using Kovan.Domain.Entities;

namespace Kovan.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CustomerType == Kovan.Domain.Enums.CustomerType.Individual ? $"{src.FirstName} {src.LastName}" : src.Title));

        CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? (src.Customer.Title ?? $"{src.Customer.FirstName} {src.Customer.LastName}") : string.Empty));

        CreateMap<InvoiceLine, InvoiceLineDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));

        CreateMap<Supplier, SupplierDto>();

        CreateMap<PurchaseOrder, PurchaseOrderDto>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty));
    }
}
