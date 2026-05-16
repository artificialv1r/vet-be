namespace Exam.App.Domain;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int AnimalSpeciesId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int OwnerId { get; set; }
    public int? VetId { get; set; }
    public AnimalSpecies AnimalSpecies { get; set; }
    public Owner Owner { get; set; }
    public Vet? Vet { get; set; }
}