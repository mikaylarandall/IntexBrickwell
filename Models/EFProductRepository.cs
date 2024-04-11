namespace IntexBrickwell.Models;
using IntexBrickwell.Data;
public class EFProductRepository : IProductRepository
{
    private ApplicationDbContext _context;
    
    public EFProductRepository(ApplicationDbContext temp)
    {
        _context = temp;
    }
    public IEnumerable<Product> Products => _context.Products;

    public void AddProduct(Product product)
    {
        _context.Add(product);
        _context.SaveChanges();
    }

    public void EditProduct(Product product)
    {
        _context.Update(product);
        _context.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
    }
}