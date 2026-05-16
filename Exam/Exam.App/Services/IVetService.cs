using Exam.App.Domain;

namespace Exam.App.Services;

public interface IVetService
{
    Task<Vet?> FindVetById(int id);
}