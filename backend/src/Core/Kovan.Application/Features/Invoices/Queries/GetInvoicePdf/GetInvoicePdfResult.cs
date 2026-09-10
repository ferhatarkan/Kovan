namespace Kovan.Application.Features.Invoices.Queries.GetInvoicePdf;

public class GetInvoicePdfResult
{
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "application/pdf";
    public string FileName { get; set; } = "fatura.pdf";
}