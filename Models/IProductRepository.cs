namespace IntexBrickwell.Models;

public interface IProductRepository
{
    public IEnumerable<Product> Products { get; }

    public void AddProduct(Product product);

    public void EditProduct(Product product);

    public void DeleteProduct(Product product);
    IEnumerable<Product> GetTopRatedProducts(int count);  // Ensure this is included
    IEnumerable<Product> GetProductsByNames(List<string> names);  // Add this line
    int GetCountOfTopRatedProducts();  // Add this line

}