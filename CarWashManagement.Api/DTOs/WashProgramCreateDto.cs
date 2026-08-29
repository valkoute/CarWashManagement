using System.ComponentModel.DataAnnotations;

namespace CarWashManagement.Api.DTOs;

public class WashProgramCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Range(0.01, 1000)]
    public decimal Price { get; set; }

    [Range(1, 120)]
    public int DurationMinutes { get; set; }
}