namespace IntexBrickwell.Models;

public interface ICustomerRecommendationRepository
{
    IEnumerable<CustomerRecommendations> CustomerRecommendations { get; }
}