using Exam.App.Services.Dtos;

namespace Exam.App.Services;

public interface IExaminationService
{
    Task<CreateExaminationDto> CreateExamination(CreateExaminationDto patient);
    Task<ExaminationPreviewDto> CancelExamination(int id,string username, CancelExaminationDto dto);
    Task<ExaminationPreviewDto> Get(int id);
}