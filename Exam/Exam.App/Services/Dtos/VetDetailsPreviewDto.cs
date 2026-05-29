namespace Exam.App.Services.Dtos;

public class VetDetailsPreviewDto
{
    public string FullName { get; set; }
    public int Id { get; set; }
    public List<ExaminationPreviewDto> Examinations { get; set; }
}