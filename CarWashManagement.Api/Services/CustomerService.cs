using CarWashManagement.Api.Models;

namespace CarWashManagement.Api.Services;

public class CustomerService
{
    private readonly List<Customer> _customers = new();

    public List<Customer> GetAll()
    {
        return _customers;
    }

    public Customer? GetById(Guid id)
{
    return _customers.FirstOrDefault(customer => customer.Id == id);
}
    public Customer Add(Customer customer)
    {
        customer.Id = Guid.NewGuid();

        _customers.Add(customer);

        return customer;
    }
}