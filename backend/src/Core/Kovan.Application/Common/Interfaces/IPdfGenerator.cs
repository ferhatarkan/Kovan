using Kovan.Domain.Entities;
using Kovan.Application.Features.Invoices.Queries.GetInvoiceById;

namespace Kovan.Application.Common.Interfaces;

public interface IPdfGenerator
{
    byte[] GenerateProductLabelPdf(Product product);
    byte[] GenerateInvoicePdf(GetInvoiceByIdResult invoice, string? logoPath);
}