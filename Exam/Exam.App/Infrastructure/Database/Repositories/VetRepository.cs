using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories;

public class VetRepository : IVetRepository
{
    private readonly AppDbContext _context;
    
    public  VetRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Vet?> GetVetById(int id)
    {
        return _context.Vets
            .Include(v => v.User)
            .FirstOrDefault(vet => vet.Id == id);
    }
}