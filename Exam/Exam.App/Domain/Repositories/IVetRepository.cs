namespace Exam.App.Domain.Repositories;

public interface IVetRepository
{
    Task<Vet?> GetVetById(int id);
}