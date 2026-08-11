using Kovan.Application.Common.Models;
using Kovan.Application.Features.Invoices.Queries;

namespace Kovan.Application.Common.Interfaces;

public interface IPdfGenerator
{
    PdfFileDto GenerateInvoicePdf(InvoiceDto invoice, string? tenantLogoPath);
}