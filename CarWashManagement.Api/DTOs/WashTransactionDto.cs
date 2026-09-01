namespace CarWashManagement.Api.DTOs;

public class WashTransactionDto
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public string LicensePlate { get; set; } = string.Empty;

    public int WashProgram { get; set; }

    public int StationNumber { get; set; }

    public int Status { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}