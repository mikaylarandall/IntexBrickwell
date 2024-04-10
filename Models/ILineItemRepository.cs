namespace IntexBrickwell.Models;

public interface ILineItemRepository
{
    public IQueryable<LineItem> LineItems { get; }
}