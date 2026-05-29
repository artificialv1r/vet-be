using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories;

public class ExaminationRepository : IExaminationRepository
{
    private readonly AppDbContext _context;

    public ExaminationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Examination> Create(Examination examination)
    {
        await _context.Examinations.AddAsync(examination);
        await _context.SaveChangesAsync();
        return examination;
    }

    public async Task<Examination> Update(Examination examination)
    {
        _context.Examinations.Update(examination);
        await _context.SaveChangesAsync();
        return examination;
    }

    public async Task<Examination> Get(int id)
    {
        var examination = await _context.Examinations
            .Include(e => e.Vet)
            .ThenInclude(v => v.User)
            .Include(e => e.Patient)
            .ThenInclude(p => p.AnimalSpecies)
            .Include(e => e.Report)
            .FirstOrDefaultAsync(e => e.Id == id);
        
        return examination;
    }
}