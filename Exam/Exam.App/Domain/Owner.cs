namespace Exam.App.Domain;

public class Owner
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public List<Patient> Pets { get; set; } = [];
}