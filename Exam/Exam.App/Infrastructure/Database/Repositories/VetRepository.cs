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
        return await _context.Vets
            .Include(v => v.User)
            .Include(v => v.Patients)
            .Include(v => v.Examinations)
            .ThenInclude(e => e.Patient)
            .ThenInclude(p => p.AnimalSpecies)
            .FirstOrDefaultAsync(vet => vet.Id == id);
    }

    public async Task<List<Vet>> GetAll()
    {
        return await _context.Vets
            .Include(v => v.User)
            .Include(v => v.Patients)
            .Include(v => v.Examinations)
            .ThenInclude(e => e.Patient)
            .ToListAsync();
    }

    public async Task<Vet?> GetByUsername(string username)
    {
        return await _context.Vets
            .Include(v => v.User)
            .Include(v => v.Patients)
            .Include(v => v.Examinations)
            .ThenInclude(e => e.Patient)
            .ThenInclude(p => p.AnimalSpecies)
            .FirstOrDefaultAsync(vet => vet.User.UserName == username);
    }
}