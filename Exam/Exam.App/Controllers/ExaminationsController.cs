using System.Security.Claims;
using Exam.App.Domain;
using Exam.App.Services;
using Exam.App.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers.Middleware;

[Route("api/[controller]")]
[ApiController]
public class ExaminationsController : ControllerBase
{
    private readonly IExaminationService _examinationService;

    public ExaminationsController(IExaminationService examinationService)
    {
        _examinationService = examinationService;
    }

    [Authorize(Roles = "Vet, Assistant")]
    [HttpPost]
    public async Task<ActionResult<CreateExaminationDto>> CreateExamination(CreateExaminationDto examinationDto)
    {
        if (!ModelState.IsValid)
        {
            return  BadRequest(ModelState);
        }

        var savedExamination = await _examinationService.CreateExamination(examinationDto);
        return Created(string.Empty, savedExamination);
    }

    [Authorize(Roles = "Vet")]
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelExamination(int id, [FromBody] CancelExaminationDto examinationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        string username = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(await _examinationService.CancelExamination(id, username, examinationDto));
    }

    [Authorize(Roles = "Vet, Assistant")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ExaminationPreviewDto>> FetchExamination(int id)
    {
        return Ok(await _examinationService.Get(id));
    }
}