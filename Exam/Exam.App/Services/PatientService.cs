using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Exam.App.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPatientRepository _patientRepository;
    private readonly IOwnerService _ownerService;
    private readonly IVetService _vetService;
    private readonly IMapper _mapper;
    
    public PatientService(IUnitOfWork unitOfWork, IPatientRepository patientRepository, IOwnerService ownerService, 
        IVetService vetService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _patientRepository = patientRepository;
        _ownerService = ownerService;
        _vetService = vetService;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PatientPreviewDto>> GetFilteredPatients(int page, PatientSearchQuery query)
    {
        int pageSize = 10;
        var patients = await _patientRepository.GetAllFilteredPatients(page, query);
        var dtos = patients.Items
            .Select(_mapper.Map<PatientPreviewDto>).ToList();
        return new PaginatedList<PatientPreviewDto>(dtos, patients.Count, patients.PageIndex, pageSize);
    }

    public async Task<PatientPreviewDto> Get(int patientId)
    {
        var patient = await _patientRepository.GetPatientById(patientId);
        if (patient == null)
        {
            throw new NotFoundException(patientId);
        }

        return _mapper.Map<PatientPreviewDto>(patient);
    }

    public async Task<CreatePatientDto> CreateNewPatient(CreatePatientDto dto)
    {
        var owner = await _ownerService.FindByUsernameAsync(dto.OwnerUsername);
        if (owner == null)
        {
            throw new BadRequestException("Owner not found");
        }

        Patient newPatient = new Patient()
        {
            Name = dto.Name,
            AnimalSpeciesId = dto.AnimalSpeciesId,
            DateOfBirth = dto.DateOfBirth,
            OwnerId = owner.Id
        };
        
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _patientRepository.CreatePatient(newPatient);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        return dto;
    }

    public async Task<UpdatePatientDto> UpdatePatient(int id, UpdatePatientDto dto)
    {
        Patient existingPatient = await _patientRepository.GetPatientById(id);

        if (existingPatient == null)
        {
            throw new NotFoundException(id);
        }

        if (dto.VetId.HasValue)
        {
            var vet = await _vetService.FindVetById(dto.VetId.Value);

            if (vet == null)
            {
                throw new NotFoundException(dto.VetId.Value);
            }
            
            existingPatient.VetId = dto.VetId;
        }
        
        existingPatient.Name = dto.Name;
        existingPatient.DateOfBirth = dto.DateOfBirth;
        
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            await _patientRepository.UpdatePatient(existingPatient);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
        
        return _mapper.Map<UpdatePatientDto>(existingPatient);
    }

    public async Task<bool> DeletePatient(int id)
    {
        var patient = await _patientRepository.GetPatientById(id);

        if (patient == null)
        {
            throw new NotFoundException(id);
        }

        if (patient.Examinations.Any())
        {
            throw new InvalidOperationException("Patient cannot be deleted because examinations exist.");
        }
        
        return await _patientRepository.DeletePatient(patient);
    }
}