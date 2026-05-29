using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Exam.App.Services;
using Exam.App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<PatientPreviewDto>>> FetchAllPatients()
    {
        return Ok(await _patientService.GetAll());
    }

    [HttpGet("filter")]
    public async Task<ActionResult<PaginatedList<PatientPreviewDto>>> GetPatientsFiltered(
        [FromQuery] PatientSearchQuery query, [FromQuery] int page = 1)
    {
        if (page < 1)
        {
            throw new BadRequestException("Page must be greater than or equal to 1");
        }
        
        return Ok(await _patientService.GetFilteredPatients(page, query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientPreviewDto>> GetPatient(int id)
    {
        return Ok(await _patientService.Get(id));
    }
    
    [Authorize(Roles = "Vet, Assistant")]
    [HttpPost]
    public async Task<ActionResult<CreatePatientDto>> PostPatient(CreatePatientDto patient)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var savedPatient = await _patientService.CreateNewPatient(patient);
        return Created(string.Empty, savedPatient);
    }

    [Authorize(Roles = "Vet, Assistant")]
    [HttpPut("{id}")]
    public async Task<ActionResult<UpdatePatientDto>> UpdatePatient(int id, UpdatePatientDto patient)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        return Ok(await _patientService.UpdatePatient(id, patient));
    }

    [Authorize(Roles = "Vet, Assistant")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePatient(int id)
    {
       
            await _patientService.DeletePatient(id);
            return NoContent();
        
    }
}