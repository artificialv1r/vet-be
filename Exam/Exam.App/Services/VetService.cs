using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;

namespace Exam.App.Services;

public class VetService : IVetService
{
    private readonly IVetRepository _vetRepository;
    private readonly IMapper _mapper;
    
    public VetService(IVetRepository vetRepository,  IMapper mapper)
    {
        _vetRepository = vetRepository;
        _mapper = mapper;
    }

    public async Task<VetDetailsPreviewDto?> FindVetById(int id)
    {
        var vet = await _vetRepository.GetVetById(id);
        return _mapper.Map<VetDetailsPreviewDto>(vet);
    }

    public async Task<List<VetPreviewDto>> FindAll()
    {
        var vets = await _vetRepository.GetAll();
        return _mapper.Map<List<VetPreviewDto>>(vets);
    }

    public async Task<VetDetailsPreviewDto?> FindByUsername(string username)
    {
        var vet = await _vetRepository.GetByUsername(username);
        if (vet == null)
        {
            throw new UserNotFoundException(username);
        }
        return _mapper.Map<VetDetailsPreviewDto>(vet);
    }
}