namespace Exam.App.Domain;

public class Examination
{
    public int Id { get; set; }
    public DateTime ExaminationDate { get; set; }
    public ExaminationStatus Status { get; set; }
    public string? CancellationReason { get; set; }
    public int VetId { get; set; }
    
    public int PatientId { get; set; }
    public Vet Vet { get; set; }
    public Patient Patient { get; set; }
    public ExamReport? Report { get; set; }
}