using Exam.App.Domain;
using Exam.App.Domain.Repositories;

namespace Exam.App.Services;

public class AnimalSpeciesService : IAnimalSpeciesService
{
    private readonly IAnimalSpeciesRepository _animalSpeciesRepository;

    public AnimalSpeciesService(IAnimalSpeciesRepository animalSpeciesRepository)
    {
        _animalSpeciesRepository = animalSpeciesRepository;
    }

    public async Task<List<AnimalSpecies>> GetAll()
    {
        return await _animalSpeciesRepository.GetAll();
    }
}