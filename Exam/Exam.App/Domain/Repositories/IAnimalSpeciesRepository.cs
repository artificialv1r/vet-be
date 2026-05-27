namespace Exam.App.Domain.Repositories;

public interface IAnimalSpeciesRepository
{
    Task<List<AnimalSpecies>> GetAll();
}