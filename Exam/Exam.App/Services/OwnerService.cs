using AutoMapper;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;

namespace Exam.App.Services;

public class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public OwnerService(IOwnerRepository ownerRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
    }

    public async Task<OwnerPreviewDto> FindByUsernameAsync(string username)
    {
        var owner = await _ownerRepository.FindByUsernameAsync(username);

        if (owner == null)
        {
            throw new Exception($"Owner with username {username} not found");
        }
        
        return _mapper.Map<OwnerPreviewDto>(owner);
    }
}