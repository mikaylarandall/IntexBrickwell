using IntexBrickwell.Data;

namespace IntexBrickwell.Models;

public class EFRecommendationRepository : IRecommendationRepository
{
    private ApplicationDbContext _context;
    
    public EFRecommendationRepository(ApplicationDbContext temp)
    {
        _context = temp;
    }
    public IEnumerable<ProductRecommendation> ProductRecommendation => _context.ProductRecommendation;
}