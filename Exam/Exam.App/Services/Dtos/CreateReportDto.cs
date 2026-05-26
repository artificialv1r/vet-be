using Exam.App.Domain;

namespace Exam.App.Services.Dtos;

public class CreateReportDto
{
    public double PatientWeight { get; set; }
    public string Description { get; set; }
    public int ExaminationId { get; set; }
}