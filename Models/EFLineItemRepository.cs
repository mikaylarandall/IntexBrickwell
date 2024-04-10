namespace IntexBrickwell.Models;
using IntexBrickwell.Data;
public class EFLineItemRepository : ILineItemRepository
{
    private ApplicationDbContext _context;

    public EFLineItemRepository(ApplicationDbContext temp)
    {
        _context = temp;
    }
    public IQueryable<LineItem> LineItems => _context.LineItems;
}