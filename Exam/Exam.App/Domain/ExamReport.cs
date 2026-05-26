namespace Exam.App.Domain;

public class ExamReport
{
    public int Id { get; set; }
    public double PatientWeight { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int ExaminationId { get; set; }
    public Examination Examination { get; set; }

    public bool CanUpdate()
    {
        int workingDays = 0;
        var currentDate = CreatedAt.Date;

        while (workingDays < 3)
        {
            currentDate = currentDate.AddDays(1);

            if (currentDate.DayOfWeek != DayOfWeek.Saturday &&
                currentDate.DayOfWeek != DayOfWeek.Sunday)
            {
                workingDays++;
            }
        }

        return DateTime.UtcNow <= currentDate;
    }
}