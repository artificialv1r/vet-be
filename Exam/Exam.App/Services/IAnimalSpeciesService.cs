using Exam.App.Domain;

namespace Exam.App.Services;

public interface IAnimalSpeciesService
{
    Task<List<AnimalSpecies>> GetAll();
}