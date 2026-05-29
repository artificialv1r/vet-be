using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;

namespace Exam.App.Services;

public class ExaminationService : IExaminationService
{
    private readonly IExaminationRepository _examinationRepository;
    private readonly IVetRepository _vetRepository;
    private readonly IMapper _mapper;

    public ExaminationService(IExaminationRepository examinationRepository,  IVetRepository vetRepository, IMapper mapper)
    {
        _examinationRepository = examinationRepository;
        _vetRepository = vetRepository;
        _mapper = mapper;
    }

    public async Task<CreateExaminationDto> CreateExamination(CreateExaminationDto examination)
    {
        try
        {
            var isVetAvailable = await IsAvailable(examination.ExaminationDate, examination.VetId);

            if (!isVetAvailable)
            {
                throw new BadRequestException("Vet is not available");
            }

            var vet = await _vetRepository.GetVetById(examination.VetId);

            if (!vet.Patients.Any(p => p.Id == examination.PatientId))
            {
                throw new NotFoundException(examination.PatientId);
            }

            Examination newExamination = new Examination()
            {
                ExaminationDate = examination.ExaminationDate,
                Status = ExaminationStatus.Active,
                VetId = examination.VetId,
                PatientId = examination.PatientId
            };

            await _examinationRepository.Create(newExamination);
            return examination;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException?.Message);

            throw;
        }
    }

    public async Task<ExaminationPreviewDto> CancelExamination(int id, string username, CancelExaminationDto dto)
    {
        var examination = await _examinationRepository.Get(id);
        if (examination == null)
        {
            throw new NotFoundException(id);
        }
        
        var vet = await _vetRepository.GetVetById(examination.VetId);
        if (vet.User.UserName != username)
        {
            throw new UnauthorizedAccessException();
        }

        examination.Status = ExaminationStatus.Cancelled;
        examination.CancellationReason = dto.CancellationReason;
        
        var updatedExamination = await _examinationRepository.Update(examination);
        return _mapper.Map<ExaminationPreviewDto>(updatedExamination);
    }

    public async Task<ExaminationPreviewDto> Get(int id)
    {
        var exam = await _examinationRepository.Get(id);
        if (exam == null)
        {
            throw new NotFoundException(id);
        }

        return _mapper.Map<ExaminationPreviewDto>(exam);
    }
    
    private async Task<bool> IsAvailable(DateTime newDate, int vetId)
    {
        var vet = await _vetRepository.GetVetById(vetId);

        if (vet == null)
        {
            throw new NotFoundException(vetId);
        }

        var examinations = vet.Examinations.Where(e => e.Status == ExaminationStatus.Active);
        
        var newExamEnd = newDate.AddMinutes(20);

        foreach (var exam in examinations)
        {
            var existingStart = exam.ExaminationDate;
            var existingEnd = exam.ExaminationDate.AddMinutes(20);
            
            bool overlaps = newDate < existingEnd && existingStart < newExamEnd;

            if (overlaps)
            {
                return false;
            }
        }
        
        return true;
    }
}