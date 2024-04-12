namespace IntexBrickwell.Models;

public interface IRecommendationRepository
{
    IEnumerable<ProductRecommendation> ProductRecommendation { get; }
    ProductRecommendation GetProductRecommendations(byte productId);

}