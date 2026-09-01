namespace CarWashManagement.Api.Models;

public class WashTransaction
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public string LicensePlate { get; set; } = string.Empty;
    public Vehicle Vehicle { get; set; } = null!;

    public WashProgramType WashProgram { get; set; }

    public int StationNumber { get; set; }
    public WashStation WashStation { get; set; } = null!;

    public WashTransactionStatus Status { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}