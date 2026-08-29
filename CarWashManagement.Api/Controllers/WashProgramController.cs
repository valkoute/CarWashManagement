using CarWashManagement.Api.DTOs;
using CarWashManagement.Api.Models;
using CarWashManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarWashManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WashProgramController : ControllerBase
{
    private readonly WashProgramService _washProgramService;

    public WashProgramController(WashProgramService washProgramService)
    {
        _washProgramService = washProgramService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var programs = await _washProgramService.GetAllAsync();

        return Ok(programs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var program = await _washProgramService.GetByIdAsync(id);

        if (program == null)
        {
            return NotFound();
        }

        return Ok(program);
    }

    [HttpPost]
    public async Task<IActionResult> Create(WashProgramCreateDto dto)
    {
        var program = new WashProgram
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            DurationMinutes = dto.DurationMinutes
        };

        var createdProgram = await _washProgramService.AddAsync(program);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProgram.Id },
            createdProgram);
    }
}