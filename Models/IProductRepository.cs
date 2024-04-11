namespace IntexBrickwell.Models;

public interface IProductRepository
{
    public IEnumerable<Product> Products { get; }

    public void AddProduct(Product product);

    public void EditProduct(Product product);

    public void DeleteProduct(Product product);
}