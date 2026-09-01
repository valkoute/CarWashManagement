using System.ComponentModel.DataAnnotations;
using CarWashManagement.Api.Models;

namespace CarWashManagement.Api.DTOs;

public class WashTransactionCreateDto
{
    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    public string LicensePlate { get; set; } = string.Empty;

    [Required]
    public WashProgramType WashProgram { get; set; }
}