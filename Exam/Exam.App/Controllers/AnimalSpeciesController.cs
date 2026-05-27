using Exam.App.Domain;
using Exam.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AnimalSpeciesController : ControllerBase
{
    private readonly IAnimalSpeciesService _animalSpeciesService;

    public AnimalSpeciesController(IAnimalSpeciesService animalSpeciesService)
    {
        _animalSpeciesService = animalSpeciesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AnimalSpecies>>> FetchSpecies()
    {
        return await _animalSpeciesService.GetAll();
    }
}