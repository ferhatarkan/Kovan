using MediatR;
using System;
using System.Collections.Generic;

namespace Kovan.Application.Features.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommand : IRequest<Guid>
{
    public Guid CustomerId { get; set; }
    public Guid WarehouseId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public List<CreateInvoiceLineDto> InvoiceLines { get; set; } = new();
}