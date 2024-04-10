namespace IntexBrickwell.Models;

public interface ICustomerRepository
{
    public IQueryable<Customer> Customers { get; }
}