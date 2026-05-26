namespace Exam.App.Domain.Repositories;

public interface IExaminationRepository
{
    Task<Examination> Create(Examination examination);
    Task<Examination> Get(int id);
    Task<Examination> Update(Examination examination);
}