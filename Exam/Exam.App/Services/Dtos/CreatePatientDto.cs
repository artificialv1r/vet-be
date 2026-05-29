namespace Exam.App.Services.Dtos;

public class CreatePatientDto
{
    public string Name { get; set; }
    public int AnimalSpeciesId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string OwnerUsername { get; set; }
    public int? VetId { get; set; }
}