using Exam.App.Services.Dtos;
using Exam.App.Utils;

namespace Exam.App.Domain.Repositories;

public interface IPatientRepository
{
    Task<List<Patient>> GetAllPatients();
    Task<Patient> GetPatientById(int id);
    Task<Patient> CreatePatient(Patient patient); 
    Task<Patient> UpdatePatient(Patient patient);
    Task<bool> DeletePatient(Patient patient);
    Task<PaginatedList<Patient>> GetAllFilteredPatients(int page, PatientSearchQuery patientSearchQuery);
}