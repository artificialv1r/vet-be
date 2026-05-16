using Exam.App.Domain;
using Exam.App.Domain.Repositories;

namespace Exam.App.Services;

public class VetService : IVetService
{
    private readonly IVetRepository _vetRepository;
    
    public VetService(IVetRepository vetRepository)
    {
        _vetRepository = vetRepository;
    }

    public async Task<Vet?> FindVetById(int id)
    {
        return await _vetRepository.GetVetById(id);
    }
}