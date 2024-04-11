using IntexBrickwell.Data;

namespace IntexBrickwell.Models;

public class EFCustomerRecommendationRepository : ICustomerRecommendationRepository
{
    private ApplicationDbContext _context;
    
    public EFCustomerRecommendationRepository(ApplicationDbContext temp)
    {
        _context = temp;
    }
    public IEnumerable<CustomerRecommendations> CustomerRecommendations => _context.CustomerRecommendations;
}