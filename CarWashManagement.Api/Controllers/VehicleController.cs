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

    [HttpGet("{licensePlate}")]
    public async Task<IActionResult> GetById(string licensePlate)
    {
        var vehicle = await _vehicleService.GetByIdAsync(licensePlate);

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
            new { licensePlate = createdVehicle.LicensePlate },
            createdVehicle);
    }
    [HttpDelete("{licensePlate}")]
    public async Task<IActionResult> Delete(string licensePlate)
    {
        var deleted = await _vehicleService.DeleteAsync(licensePlate);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomerId(Guid customerId)
    {
        var vehicles = await _vehicleService.GetByCustomerIdAsync(customerId);

        return Ok(vehicles);
    }
}