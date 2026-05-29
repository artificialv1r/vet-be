using Exam.App.Domain;
using Exam.App.Services;
using Exam.App.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VetsController : ControllerBase
{
    private readonly IVetService _vetService;

    public VetsController(IVetService vetService)
    {
        _vetService = vetService;
    }

    [HttpGet]
    public Task<List<VetPreviewDto>> FindAll()
    {
        return _vetService.FindAll();
    }

    [HttpGet("{vetId}")]
    public Task<VetDetailsPreviewDto> FindById(int vetId)
    {
        return _vetService.FindVetById(vetId);
    }

    [HttpGet("q")]
    public async Task<VetDetailsPreviewDto> FetchByUsername(string username)
    {
        return await _vetService.FindByUsername(username);
    }
}