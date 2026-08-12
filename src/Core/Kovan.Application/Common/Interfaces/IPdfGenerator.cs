using Kovan.Domain.Entities;
using Kovan.Application.Features.Invoices.Dtos; // DTO'ların yeni ortak namespace'i

namespace Kovan.Application.Common.Interfaces;

public interface IPdfGenerator
{
    byte[] GenerateProductLabelPdf(Product product);
    byte[] GenerateInvoicePdf(InvoiceDto invoice, string? logoPath);
}