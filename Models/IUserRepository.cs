namespace IntexBrickwell.Models;

public interface IUserRepository
{
    public IQueryable<User> Users { get; }
}