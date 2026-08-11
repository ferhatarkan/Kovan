using Kovan.Application.Features.Suppliers.Commands.CreateSupplier;
using Kovan.Application.Features.Suppliers.Commands.DeleteSupplier;
using Kovan.Application.Features.Suppliers.Commands.UpdateSupplier;
using Kovan.Application.Features.Suppliers.Queries.GetSuppliers;
using Kovan.Application.Features.Suppliers.Queries;
using MediatR;
using Kovan.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly ISender _sender;

    public SuppliersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSupplierCommand command)
    {
        var supplierId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = supplierId }, new { SupplierId = supplierId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await _sender.Send(new GetSuppliersQuery());
        return Ok(suppliers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var supplier = await _sender.Send(new GetSupplierByIdQuery { Id = id });
        return Ok(supplier);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateSupplierCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Route ID must match command ID.");
        }

        await _sender.Send(command);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteSupplierCommand { Id = id });

        return NoContent();
    }
}