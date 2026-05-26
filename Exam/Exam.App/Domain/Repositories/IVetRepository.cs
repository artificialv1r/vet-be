namespace Exam.App.Domain.Repositories;

public interface IVetRepository
{
    Task<Vet?> GetVetById(int id);
    Task<List<Vet>> GetAll();
    Task<Vet?> GetByUsername(string username);
}