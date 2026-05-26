using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;
    
    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ExamReport> Create(ExamReport report)
    {
        try
        {

            await _context.ExamReports.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;   
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException?.Message);
            throw;
        }
    }

    public async Task<ExamReport> Update(ExamReport report)
    {
        try
        {

            _context.ExamReports.Update(report);
            await _context.SaveChangesAsync();
            return report;   
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException?.Message);
            throw;
        }
    }

    public async Task<ExamReport?> Get(int reportId)
    {
        return await _context.ExamReports
            .Include(er => er.Examination)
            .ThenInclude(er => er.Vet)
            .FirstOrDefaultAsync(er => er.Id == reportId);
    }
}