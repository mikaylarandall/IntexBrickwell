namespace IntexBrickwell.Models;

public interface IProductRepository
{
    public IEnumerable<Product> Products { get; }
}