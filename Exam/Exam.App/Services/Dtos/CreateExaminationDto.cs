namespace Exam.App.Services.Dtos;

public class CreateExaminationDto
{
    public DateTime ExaminationDate { get; set; }
    public int PatientId { get; set; }
    public int VetId { get; set; }
}