using CarWashManagement.Api.Data;
using CarWashManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagement.Api.Services;

public class WashProgramService
{
    private readonly CarWashDbContext _context;

    public WashProgramService(CarWashDbContext context)
    {
        _context = context;
    }

    public async Task<List<WashProgram>> GetAllAsync()
    {
        return await _context.WashPrograms
            .ToListAsync();
    }

    public async Task<WashProgram?> GetByIdAsync(Guid id)
    {
        return await _context.WashPrograms
            .FirstOrDefaultAsync(program => program.Id == id);
    }

    public async Task<WashProgram> AddAsync(WashProgram program)
    {
        _context.WashPrograms.Add(program);
        await _context.SaveChangesAsync();

        return program;
    }
}