namespace IntexBrickwell.Models;

public interface IOrderRepository
{
    public IQueryable<Order> Orders { get; }

    public void AddOrder(Order order);
}