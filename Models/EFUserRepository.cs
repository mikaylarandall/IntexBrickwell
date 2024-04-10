namespace IntexBrickwell.Models;
using IntexBrickwell.Data;
public class EFUserRepository : IUserRepository
{
    private ApplicationDbContext _context;
    
    public EFUserRepository(ApplicationDbContext temp)
    {
        _context = temp;
    }
    public IQueryable<User> Users => _context.Users;
}