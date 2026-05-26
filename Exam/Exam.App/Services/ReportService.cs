using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;

namespace Exam.App.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IExaminationRepository _examinationRepository;
    private readonly IVetRepository _vetRepository;
    private readonly IMapper _mapper;

    public ReportService(IReportRepository reportRepository, IExaminationRepository examinationRepository, 
        IVetRepository vetRepository, IMapper 
            mapper)
    {
        _reportRepository = reportRepository;
        _examinationRepository = examinationRepository;
        _vetRepository = vetRepository;
        _mapper = mapper;
    }

    public async Task<CreateReportDto> Create(CreateReportDto report, string username)
    {
        var exam = await _examinationRepository.Get(report.ExaminationId);
        if (exam == null)
        {
            throw new NotFoundException(report.ExaminationId);
        }

        if (exam.Report != null)
        {
            throw new InvalidOperationException("Report already exists for this examination.");
        }

        if (exam.Status == ExaminationStatus.Cancelled)
        {
            throw new InvalidOperationException($"This examination is cancelled!");
        }

        var vet = await _vetRepository.GetByUsername(username);
        if (exam.VetId != vet.Id)
        {
            throw new UnauthorizedAccessException();
        }
        
        var mappedReport = _mapper.Map<ExamReport>(report);
        Console.WriteLine(mappedReport.ExaminationId);
        var newReport = await _reportRepository.Create(mappedReport);
        exam.Status = ExaminationStatus.Completed;
        await _examinationRepository.Update(exam);
        return report;
    }

    public async Task<UpdateReportDto> Update(UpdateReportDto report,  int reportId)
    {
        var existingReport = await _reportRepository.Get(reportId);
        if(existingReport == null)
        {
            throw new NotFoundException(reportId);
        }

        if(!existingReport.CanUpdate())
        {
            throw new InvalidOperationException($"Time out. Can't update report.");
        }

        var exam = await _examinationRepository.Get(existingReport.ExaminationId);
        if (exam == null)
        {
            throw new NotFoundException(existingReport.ExaminationId);
        }

        if (exam.Status == ExaminationStatus.Cancelled)
        {
            throw new InvalidOperationException($"This examination is cancelled!");
        }

        existingReport.PatientWeight = report.PatientWeight;
        existingReport.Description = report.Description;
        
        await _reportRepository.Update(existingReport);
        return report;
    }
}