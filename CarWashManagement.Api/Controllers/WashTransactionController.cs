using CarWashManagement.Api.DTOs;
using CarWashManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;
namespace CarWashManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WashTransactionController : ControllerBase
{
    private readonly WashTransactionService _washTransactionService;

    public WashTransactionController(WashTransactionService washTransactionService)
    {
        _washTransactionService = washTransactionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transactions = await _washTransactionService.GetAllAsync();

        return Ok(transactions);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var transaction = await _washTransactionService.GetByIdAsync(id);

        if (transaction == null)
        {
            return NotFound();
        }

        return Ok(transaction);
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartWash(WashTransactionCreateDto dto)
    {
        var result = await _washTransactionService.StartWashAsync(
            dto.CustomerId,
            dto.LicensePlate,
            dto.WashProgram);

        if (result.VehicleNotFound)
        {
            return NotFound("Vehicle was not found for this customer.");
        }

        if (result.NoStationAvailable)
        {
            return Conflict("No wash stations are currently available.");
        }

        var transaction = result.Transaction!;

var response = new WashTransactionDto
{
    Id = transaction.Id,
    CustomerId = transaction.CustomerId,
    LicensePlate = transaction.LicensePlate,
    WashProgram = (int)transaction.WashProgram,
    StationNumber = transaction.StationNumber,
    Status = (int)transaction.Status,
    StartedAt = transaction.StartedAt,
    CompletedAt = transaction.CompletedAt
};

return CreatedAtAction(
    nameof(GetById),
    new { id = response.Id },
    response);
    }
    [HttpPost("{id:guid}/complete")]
public async Task<IActionResult> CompleteWash(Guid id)
{
    var transaction = await _washTransactionService.CompleteWashAsync(id);

    if (transaction == null)
    {
        return NotFound();
    }

    return Ok(transaction);
}
}