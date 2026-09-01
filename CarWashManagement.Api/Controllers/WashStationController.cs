using CarWashManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarWashManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WashStationController : ControllerBase
{
    private readonly WashStationService _washStationService;

    public WashStationController(WashStationService washStationService)
    {
        _washStationService = washStationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var washStations = await _washStationService.GetAllAsync();

        return Ok(washStations);
    }
    [HttpGet("{stationNumber:int}")]
    public async Task<IActionResult> GetById(int stationNumber)
    {
        var washStation = await _washStationService.GetByIdAsync(stationNumber);

        if (washStation == null)
        {
            return NotFound();
        }

        return Ok(washStation);
    }
}