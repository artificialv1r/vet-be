using Exam.App.Domain;
using Exam.App.Services.Dtos;

namespace Exam.App.Services;

public interface IVetService
{
    Task<VetDetailsPreviewDto?> FindVetById(int id);
    Task<List<VetPreviewDto>> FindAll();
    Task<VetDetailsPreviewDto?> FindByUsername(string username);
}