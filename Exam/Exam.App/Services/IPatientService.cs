using Exam.App.Services.Dtos;
using Exam.App.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Services;

public interface IPatientService
{
    Task<PaginatedList<PatientPreviewDto>> GetFilteredPatients(int page, PatientSearchQuery query);
    Task<CreatePatientDto> CreateNewPatient(CreatePatientDto patient);
    Task<UpdatePatientDto> UpdatePatient(int id, UpdatePatientDto patient);
    Task<bool> DeletePatient(int id);
}