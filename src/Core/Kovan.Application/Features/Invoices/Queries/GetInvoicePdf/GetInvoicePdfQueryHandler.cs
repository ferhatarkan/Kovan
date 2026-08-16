using Kovan.Application.Common.Interfaces;
using Kovan.Application.Features.Invoices.Queries.GetInvoiceById;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kovan.Application.Features.Invoices.Queries.GetInvoicePdf;

public class GetInvoicePdfQueryHandler : IRequestHandler<GetInvoicePdfQuery, GetInvoicePdfResult>
{
    private readonly ISender _sender;
    private readonly IPdfGenerator _pdfGenerator;

    public GetInvoicePdfQueryHandler(ISender sender, IPdfGenerator pdfGenerator)
    {
        _sender = sender;
        _pdfGenerator = pdfGenerator;
    }

    public async Task<GetInvoicePdfResult> Handle(GetInvoicePdfQuery request, CancellationToken cancellationToken)
    {
        // 1. PDF'i oluşturmak için gerekli olan tüm fatura detaylarını al.
        // Bunun için mevcut GetInvoiceByIdQuery'yi yeniden kullanıyoruz. Bu, kod tekrarını önler.
        var invoiceDetails = await _sender.Send(new GetInvoiceByIdQuery { Id = request.InvoiceId }, cancellationToken);

        // 2. Fatura detaylarını PDF oluşturucu servise gönder.
        // Logo yolu gibi ayarlar veritabanından veya IConfiguration'dan alınabilir. Şimdilik null geçiyoruz.
        var pdfBytes = _pdfGenerator.GenerateInvoicePdf(invoiceDetails, null);

        // 3. Controller'a döndürülecek sonucu hazırla.
        return new GetInvoicePdfResult
        {
            Content = pdfBytes,
            ContentType = "application/pdf",
            FileName = $"Fatura-{invoiceDetails.InvoiceNumber}.pdf"
        };
    }
}