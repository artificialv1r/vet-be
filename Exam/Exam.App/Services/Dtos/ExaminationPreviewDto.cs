namespace Exam.App.Services.Dtos;

public class ExaminationPreviewDto
{
    public string Id { get; set; }
    public DateTime ExaminationDate { get; set; }
    public string PatientName { get; set; }
    public string AnimalSpecie {get; set;}
    public int Age { get; set; }
    public string VetName { get; set; }
    public string Status { get; set; }
    public string? CancellationReason { get; set; }
}