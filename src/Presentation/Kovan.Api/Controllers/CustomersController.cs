using Kovan.Application.Features.Customers.Commands.CreateCustomer;
using Kovan.Application.Features.Customers.Commands.UpdateCustomer;
using Kovan.Application.Features.Customers.Commands.DeleteCustomer;
using Kovan.Application.Features.Customers.Queries.GetCustomerById;
using Kovan.Application.Features.Customers.Queries.GetPaginatedCustomers;
using Kovan.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ISender _sender;

    public CustomersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerCommand command)
    {
        var customerId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = customerId }, new { CustomerId = customerId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetCustomerByIdQuery { Id = id };
        var customer = await _sender.Send(query);
        return Ok(customer);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetPaginatedCustomersQuery query)
    {
        var customers = await _sender.Send(query);
        return Ok(customers);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCustomerCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Route ID must match command ID.");
        }

        await _sender.Send(command);

        // 204 No Content, güncelleme başarılı olduğunda standart yanıttır.
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteCustomerCommand { Id = id });

        return NoContent();
    }
}
