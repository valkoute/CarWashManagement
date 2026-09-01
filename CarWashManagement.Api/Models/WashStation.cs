namespace CarWashManagement.Api.Models;
public class WashStation
{
    public StationStatus Status { get; set; }
    public int StationNumber { get; set; }
    public bool IsActive { get; set; } 
}