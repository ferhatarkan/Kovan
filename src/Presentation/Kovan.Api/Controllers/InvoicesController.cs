using Kovan.Application.Features.Invoices.Commands.CreateInvoice;
using Kovan.Domain.Constants;
using Kovan.Application.Features.Invoices.Commands.UpdateInvoice;
using Kovan.Application.Features.Invoices.Commands.DeleteInvoice;
using Kovan.Application.Features.Invoices.Commands.AddPayment;
using Kovan.Application.Features.Invoices.Queries;
using Kovan.Application.Features.Invoices.Queries.GetPaginatedInvoices;
using Kovan.Application.Features.Invoices.Queries.GetInvoicePdf;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Controller seviyesinde yetkilendirme
public class InvoicesController : ControllerBase
{
    private readonly ISender _sender;

    public InvoicesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceCommand command)
    {
        var invoiceId = await _sender.Send(command);

        // Fatura başarıyla oluşturulduğunda, oluşturulan kaynağın konumunu belirten
        // bir 201 Created yanıtı döndür. Bu, RESTful API'ler için en iyi pratiktir.
        return CreatedAtAction(nameof(GetById), new { id = invoiceId }, new { InvoiceId = invoiceId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetInvoiceByIdQuery { Id = id }; // 'init' property için doğru atama.
        var invoice = await _sender.Send(query);
        return Ok(invoice);
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] GetPaginatedInvoicesQuery query)
    {
        var invoices = await _sender.Send(query);
        return Ok(invoices);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteInvoiceCommand { Id = id });

        return NoContent(); // 204 No Content, silme başarılı olduğunda standart yanıttır.
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateInvoiceCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Route ID must match command ID.");
        }

        await _sender.Send(command);

        return NoContent();
    }

    [HttpPost("{invoiceId:guid}/payments")]
    public async Task<IActionResult> AddPayment(Guid invoiceId, [FromBody] AddPaymentCommand command)
    {
        command.InvoiceId = invoiceId;

        await _sender.Send(command);

        return NoContent(); // 204 No Content, işlemin başarıyla tamamlandığını belirtir.
    }

    [HttpGet("{id:guid}/pdf")]
    public async Task<IActionResult> GetInvoicePdf(Guid id)
    {
        var query = new GetInvoicePdfQuery { InvoiceId = id };
        var pdfFile = await _sender.Send(query);

        return File(pdfFile.Content, pdfFile.ContentType, pdfFile.FileName);
    }
}
