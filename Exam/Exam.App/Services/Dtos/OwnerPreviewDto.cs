using Exam.App.Domain;

namespace Exam.App.Services.Dtos;

public class OwnerPreviewDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public List<string> Pets { get; set; }
}