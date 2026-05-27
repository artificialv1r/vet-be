using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Utils;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories;

public class PatientRepository : IPatientRepository
{
    private AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Patient>> GetAllPatients()
    {
        return await  _context.Patients
            .Include(p => p.AnimalSpecies)
            .Include(p => p.Owner)
            .ThenInclude(o => o.User)
            .Include(p => p.Vet)
            .ThenInclude(v => v.User)
            .ToListAsync();
    }

    public async Task<Patient?> GetPatientById(int id)
    {
        return await _context.Patients
            .Include(p => p.AnimalSpecies)
            .Include(p => p.Owner)
            .ThenInclude(o => o.User)
            .Include(p => p.Vet)
            .ThenInclude(v => v.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Patient> CreatePatient(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        return patient;
    }

    public async Task<Patient> UpdatePatient(Patient patient)
    {
        _context.Patients.Update(patient);
        return patient;
    }

    public async Task<bool> DeletePatient(Patient patient)
    {
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PaginatedList<Patient>> GetAllFilteredPatients(int page, PatientSearchQuery patientSearchQuery)
    {
        IQueryable<Patient> patients = _context.Patients
            .Include(p => p.Owner)
            .ThenInclude(o => o.User)
            .Include(p => p.Vet)
            .ThenInclude(v => v.User)
            .Include(p => p.AnimalSpecies);
        
        patients = FilterPatients(patients, patientSearchQuery);
        
        int pageSize = 10;
        int pageIndex = page - 1;
        var count = await patients.CountAsync();
        var items = await patients.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<Patient>(items, count, pageIndex, pageSize);
    }

    private IQueryable<Patient> FilterPatients(IQueryable<Patient> patients, PatientSearchQuery patientSearchQuery)
    {
        if (!string.IsNullOrEmpty(patientSearchQuery.Vet))
        {
            patients = patients.Where(p => p.Vet.User.Name.ToLower().Contains(patientSearchQuery.Vet.ToLower()));
        }

        if (!string.IsNullOrEmpty(patientSearchQuery.Name))
        {
            patients = patients.Where(p => p.Name.ToLower().Contains(patientSearchQuery.Name.ToLower()));
        }

        if (!string.IsNullOrEmpty(patientSearchQuery.AnimalSpecies))
        {
            patients = patients.Where(p => p.AnimalSpecies.Name.ToLower().Contains(patientSearchQuery.AnimalSpecies.ToLower()));
        }

        if (patientSearchQuery.AgeFrom != null)
        {
            patients = patients.Where(p => (DateTime.Now.Year - p.DateOfBirth.Year) >= patientSearchQuery.AgeFrom);
        }
        
        if (patientSearchQuery.AgeTo != null)
        {
            patients = patients.Where(p => (DateTime.Now.Year - p.DateOfBirth.Year) <= patientSearchQuery.AgeTo);
        }
        
        return patients;
    }
    
}