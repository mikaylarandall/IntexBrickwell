namespace IntexBrickwell.Models;

public interface IOrderRepository
{
    public IQueryable<Order> Orders { get; }
}