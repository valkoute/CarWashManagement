using CarWashManagement.Api.Data;
using CarWashManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagement.Api.Services;
public class WashStationService
{
    private readonly CarWashDbContext _context;

    public WashStationService(CarWashDbContext context)
    {
        _context = context;
    }

    public async Task<List<WashStation>> GetAllAsync()
    {
        return await _context.WashStations.ToListAsync();
    }

    public async Task<WashStation?> GetByIdAsync(int stationNumber)
{
    return await _context.WashStations
        .FirstOrDefaultAsync(washStation => washStation.StationNumber == stationNumber);
}
}