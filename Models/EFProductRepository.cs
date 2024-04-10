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
}