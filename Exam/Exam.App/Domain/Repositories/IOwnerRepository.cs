namespace Exam.App.Domain.Repositories;

public interface IOwnerRepository
{
    Task<Owner?> FindByUsernameAsync(string username);
}