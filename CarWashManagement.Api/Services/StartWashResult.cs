using CarWashManagement.Api.Models;

namespace CarWashManagement.Api.Services;

public class StartWashResult
{
    public WashTransaction? Transaction { get; set; }

    public bool VehicleNotFound { get; set; }

    public bool NoStationAvailable { get; set; }
}