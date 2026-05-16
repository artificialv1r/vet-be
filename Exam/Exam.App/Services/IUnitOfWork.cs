namespace Exam.App.Services;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task SaveAsync();
}