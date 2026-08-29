using System.ComponentModel.DataAnnotations;

namespace CarWashManagement.Api.DTOs;

public class VehicleCreateDto
{
    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public string LicensePlate { get; set; } = string.Empty;

    [Required]
    public string Make { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    [Range(1900, 2026)]
    public int Year { get; set; }
}