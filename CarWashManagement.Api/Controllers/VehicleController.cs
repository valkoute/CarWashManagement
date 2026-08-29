using CarWashManagement.Api.DTOs;
using CarWashManagement.Api.Models;
using CarWashManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarWashManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly VehicleService _vehicleService;

    public VehicleController(VehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vehicles = await _vehicleService.GetAllAsync();

        return Ok(vehicles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);

        if (vehicle == null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VehicleCreateDto dto)
    {
        var vehicle = new Vehicle
        {
            CustomerId = dto.CustomerId,
            LicensePlate = dto.LicensePlate,
            Make = dto.Make,
            Model = dto.Model,
            Year = dto.Year
        };

        var createdVehicle = await _vehicleService.AddAsync(vehicle);

        if (createdVehicle == null)
        {
            return NotFound("Customer not found.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdVehicle.Id },
            createdVehicle);
    }
    [HttpGet("customer/{customerId}")]
public async Task<IActionResult> GetByCustomerId(Guid customerId)
{
    var vehicles = await _vehicleService.GetByCustomerIdAsync(customerId);

    return Ok(vehicles);
}
}