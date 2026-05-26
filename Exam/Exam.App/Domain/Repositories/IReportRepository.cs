namespace Exam.App.Domain.Repositories;

public interface IReportRepository
{
    Task<ExamReport> Create(ExamReport report);
    Task<ExamReport> Update(ExamReport report);
    Task<ExamReport?> Get(int reportId);
}