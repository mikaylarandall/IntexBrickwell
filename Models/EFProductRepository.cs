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
    
    public IEnumerable<Product> GetTopRatedProducts(int count)
    {
        var topRatedProducts = _context.Products
            .Select(p => new
            {
                Product = p,
                AverageRating = p.LineItems.Any() ? p.LineItems.Average(l => l.Rating.HasValue ? l.Rating.Value : 0) : 0
            })
            .Where(p => p.AverageRating > 0) // Filter out products without ratings
            .OrderByDescending(p => p.AverageRating)
            .Take(count)
            .Select(p => p.Product)
            .ToList();

        return topRatedProducts;
    }
    
    public IEnumerable<Product> GetProductsByNames(List<string> names)
    {
        var products = _context.Products
            .Where(p => names.Contains(p.Name))
            .ToList();

        return products;
    }
    
    public int GetCountOfTopRatedProducts()
    {
        var count = _context.Products
            .Select(p => new
            {
                Product = p,
                AverageRating = p.LineItems.Any() ? p.LineItems.Average(l => l.Rating.HasValue ? l.Rating.Value : 0) : 0
            })
            .Count(p => p.AverageRating > 0);  // Only count products with ratings

        return count;
    }

}