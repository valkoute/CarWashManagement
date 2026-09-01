using CarWashManagement.Api.Models;
using CarWashManagement.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagement.Api.Services;

public class CustomerService
{
    private readonly CarWashDbContext _context;
    public CustomerService(CarWashDbContext context)
{
    _context = context;
}

    public async Task<List<Customer>> GetAllAsync()
{
    return await _context.Customers.ToListAsync();
}
  public async Task<Customer?> GetByIdAsync(Guid id)
{
    return await _context.Customers
        .FirstOrDefaultAsync(customer => customer.Id == id);
}
public async Task<Customer> AddAsync(Customer customer)
{
    _context.Customers.Add(customer);
    await _context.SaveChangesAsync();

    return customer;
}
public async Task<bool> DeleteAsync(Guid id)
{
    var customer = await _context.Customers
        .Include(customer => customer.Vehicles)
        .FirstOrDefaultAsync(customer => customer.Id == id);

    if (customer == null)
    {
        return false;
    }

    if (customer.Vehicles.Count > 0)
    {
        return false;
    }

    _context.Customers.Remove(customer);

    await _context.SaveChangesAsync();

    return true;
}
}