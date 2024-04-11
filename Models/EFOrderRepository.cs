namespace IntexBrickwell.Models;
using IntexBrickwell.Data;
public class EFOrderRepository : IOrderRepository
{
    private ApplicationDbContext _context;
    
    public EFOrderRepository(ApplicationDbContext temp)
    {
        _context = temp;
    }
    
    public IQueryable<Order> Orders => _context.Orders;

    public void AddOrder(Order order)
    {
        _context.Add(order);
        _context.SaveChanges();
    }

}