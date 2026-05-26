using Exam.App.Services.Dtos;

namespace Exam.App.Services;

public interface IReportService
{
    Task<CreateReportDto> Create(CreateReportDto report, string username);
    Task<UpdateReportDto> Update(UpdateReportDto report, int reportId);
}