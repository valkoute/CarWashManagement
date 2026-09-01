using CarWashManagement.Api.Models;

namespace CarWashManagement.Api.Services;

public static class WashProgramDuration
{
    public static TimeSpan GetDuration(WashProgramType program)
    {
        return program switch
        {
            WashProgramType.Basic => TimeSpan.FromMinutes(5),
            WashProgramType.Premium => TimeSpan.FromMinutes(8),
            WashProgramType.Deluxe => TimeSpan.FromMinutes(12),
            _ => TimeSpan.FromMinutes(5)
        };
    }
}