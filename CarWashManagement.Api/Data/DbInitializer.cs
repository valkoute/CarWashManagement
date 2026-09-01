using CarWashManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagement.Api.Data;

public class DbInitializer
{
    private readonly CarWashDbContext _context;

    public DbInitializer(CarWashDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        if (await _context.WashStations.AnyAsync())
        {
            return;
        }

        for (int i = 1; i <= 6; i++)
        {
            var washStation = new WashStation
            {
                StationNumber = i,
                Status = StationStatus.Available,
                IsActive = true
            };

            _context.WashStations.Add(washStation);
        }

        await _context.SaveChangesAsync();
    }
}