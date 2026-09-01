using CarWashManagement.Api.DTOs;
using CarWashManagement.Api.Models;
using CarWashManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarWashManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

   [HttpGet]
public async Task<IActionResult> GetAll()
{
    var customers = await _customerService.GetAllAsync();

    return Ok(customers);
}
    [HttpGet("{id:guid}")]
public async Task<IActionResult> GetById(Guid id)
{
    var customer = await _customerService.GetByIdAsync(id);

    if (customer == null)
    {
        return NotFound();
    }

    return Ok(customer);
}

 [HttpPost]
public async Task<IActionResult> Create(CustomerCreateDto dto)
{
    var customer = new Customer
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber
    };

    var createdCustomer = await _customerService.AddAsync(customer);

    return CreatedAtAction(
        nameof(GetById),
        new { id = createdCustomer.Id },
        createdCustomer);
}
[HttpDelete("{id:guid}")]
public async Task<IActionResult> Delete(Guid id)
{
    var deleted = await _customerService.DeleteAsync(id);

    if (!deleted)
    {
        return Conflict(
            "Customer could not be deleted. Remove their vehicles first."
        );
    }

    return NoContent();
}
}