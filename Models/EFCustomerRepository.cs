namespace IntexBrickwell.Models;
using IntexBrickwell.Data;

public class EFCustomerRepository : ICustomerRepository
{
    private ApplicationDbContext _context;

    public EFCustomerRepository(ApplicationDbContext temp)
    {
        _context = temp;
    }
    public IQueryable<Customer> Customers => _context.Customers;
}