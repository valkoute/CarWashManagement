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
    public IActionResult GetAll()
    {
        var customers = _customerService.GetAll();

        return Ok(customers);
    }
    [HttpGet("{id:guid}")]
public IActionResult GetById(Guid id)
{
    var customer = _customerService.GetById(id);

    if (customer is null)
    {
        return NotFound();
    }

    return Ok(customer);
}

 [HttpPost]
public IActionResult Add(CustomerCreateDto customerDto)
{
    var customer = new Customer
    {
        FirstName = customerDto.FirstName,
        LastName = customerDto.LastName,
        Email = customerDto.Email,
        PhoneNumber = customerDto.PhoneNumber
    };

    var createdCustomer = _customerService.Add(customer);

     return CreatedAtAction(
    nameof(GetById),
    new { id = createdCustomer.Id },
    createdCustomer);
}
}