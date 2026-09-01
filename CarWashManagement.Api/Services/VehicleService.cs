using CarWashManagement.Api.Data;
using CarWashManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagement.Api.Services;

public class VehicleService
{
    private readonly CarWashDbContext _context;

    public VehicleService(CarWashDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles.ToListAsync();
    }
    public async Task<Vehicle?> GetByIdAsync(string licensePlate)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(vehicle => vehicle.LicensePlate == licensePlate);
    }
    public async Task<Vehicle?> AddAsync(Vehicle vehicle)
    {
        var customerExists = await _context.Customers
            .AnyAsync(customer => customer.Id == vehicle.CustomerId);

        if (!customerExists)
        {
            return null;
        }

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return vehicle;
    }
    public async Task<bool> DeleteAsync(string licensePlate)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(vehicle => vehicle.LicensePlate == licensePlate);

        if (vehicle == null)
        {
            return false;
        }

        _context.Vehicles.Remove(vehicle);

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<List<Vehicle>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Vehicles
            .Where(vehicle => vehicle.CustomerId == customerId)
            .ToListAsync();
    }
}