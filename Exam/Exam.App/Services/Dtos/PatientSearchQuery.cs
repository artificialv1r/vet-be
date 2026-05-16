namespace Exam.App.Services.Dtos;

public class PatientSearchQuery
{
    public string? Vet {get;set;}
    public string? Name {get;set;}
    public string? AnimalSpecies {get;set;}
    public int? AgeFrom {get;set;}
    public int? AgeTo {get;set;}
}