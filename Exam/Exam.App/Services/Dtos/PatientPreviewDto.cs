namespace Exam.App.Services.Dtos;

public class PatientPreviewDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string AnimalSpecies { get; set; }
    public string AnimalSpeciesId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }
    public string Owner { get; set; }
    public string OwnerUsername { get; set; }
    public string Vet { get; set; }
}