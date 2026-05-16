using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly AppDbContext _context;
    
    public OwnerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Owner?> FindByUsernameAsync(string username)
    {
        return await _context.Owners
            .Include(o => o.User)
            .Include(o => o.Pets)
            .ThenInclude(p => p.AnimalSpecies)
            .FirstOrDefaultAsync(o => o.User.UserName == username);
    }
}