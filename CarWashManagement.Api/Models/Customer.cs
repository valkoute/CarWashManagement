namespace CarWashManagement.Api.Models;

public class Customer
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Vehicle> Vehicles { get; set; } = new();
}