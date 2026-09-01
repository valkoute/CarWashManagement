using CarWashManagement.Api.Data;
using CarWashManagement.Api.DTOs;
using CarWashManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagement.Api.Services;

public class WashTransactionService
{
    private readonly CarWashDbContext _context;

    public WashTransactionService(CarWashDbContext context)
    {
        _context = context;
    }

    public async Task<List<WashTransactionDto>> GetAllAsync()
    {
        return await _context.WashTransactions
            .Select(transaction => new WashTransactionDto
            {
                Id = transaction.Id,
                CustomerId = transaction.CustomerId,
                LicensePlate = transaction.LicensePlate,
                WashProgram = (int)transaction.WashProgram,
                StationNumber = transaction.StationNumber,
                Status = (int)transaction.Status,
                StartedAt = transaction.StartedAt,
                CompletedAt = transaction.CompletedAt
            })
            .ToListAsync();
    }

    public async Task<WashTransactionDto?> GetByIdAsync(Guid id)
    {
        return await _context.WashTransactions
            .Where(transaction => transaction.Id == id)
            .Select(transaction => new WashTransactionDto
            {
                Id = transaction.Id,
                CustomerId = transaction.CustomerId,
                LicensePlate = transaction.LicensePlate,
                WashProgram = (int)transaction.WashProgram,
                StationNumber = transaction.StationNumber,
                Status = (int)transaction.Status,
                StartedAt = transaction.StartedAt,
                CompletedAt = transaction.CompletedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<StartWashResult> StartWashAsync(
        Guid customerId,
        string licensePlate,
        WashProgramType washProgram)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(vehicle =>
                vehicle.LicensePlate == licensePlate &&
                vehicle.CustomerId == customerId);

        if (vehicle == null)
        {
            return new StartWashResult
            {
                VehicleNotFound = true
            };
        }

        var availableStation = await _context.WashStations
            .Where(station =>
                station.Status == StationStatus.Available &&
                station.IsActive)
            .OrderBy(station => station.StationNumber)
            .FirstOrDefaultAsync();

        if (availableStation == null)
        {
            return new StartWashResult
            {
                NoStationAvailable = true
            };
        }

        var transaction = new WashTransaction
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            LicensePlate = licensePlate,
            WashProgram = washProgram,
            StationNumber = availableStation.StationNumber,
            Status = WashTransactionStatus.InProgress,
            StartedAt = DateTime.UtcNow
        };

        availableStation.Status = StationStatus.Occupied;

        _context.WashTransactions.Add(transaction);

        await _context.SaveChangesAsync();

        return new StartWashResult
        {
            Transaction = transaction
        };
    }
    public async Task<WashTransactionDto?> CompleteWashAsync(Guid id)
{
    var transaction = await _context.WashTransactions
        .Include(transaction => transaction.WashStation)
        .FirstOrDefaultAsync(transaction => transaction.Id == id);

    if (transaction == null)
    {
        return null;
    }

    if (transaction.Status != WashTransactionStatus.InProgress)
    {
        return null;
    }

    transaction.Status = WashTransactionStatus.Completed;
    transaction.CompletedAt = DateTime.UtcNow;

    transaction.WashStation.Status = StationStatus.Available;

    await _context.SaveChangesAsync();

    return new WashTransactionDto
    {
        Id = transaction.Id,
        CustomerId = transaction.CustomerId,
        LicensePlate = transaction.LicensePlate,
        WashProgram = (int)transaction.WashProgram,
        StationNumber = transaction.StationNumber,
        Status = (int)transaction.Status,
        StartedAt = transaction.StartedAt,
        CompletedAt = transaction.CompletedAt
    };
}
}