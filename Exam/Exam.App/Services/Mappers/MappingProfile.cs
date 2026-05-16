using AutoMapper;
using Exam.App.Domain;
using Exam.App.Services.Dtos;

namespace Exam.App.Services.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ProfileDto>();
            
            CreateMap<Patient, PatientPreviewDto>()
                .ForMember(
                    dest => dest.AnimalSpecies,
                    opt => opt.MapFrom(src => src.AnimalSpecies.Name))
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => DateTime.Now.Year - src.DateOfBirth.Year))
                .ForMember(
                    dest => dest.Vet,
                    opt => opt.MapFrom(src => src.Vet.User.Name + " " + src.Vet.User.Surname))
                .ForMember(
                    dest => dest.Owner,
                    opt => opt.MapFrom(src => src.Owner.User.Name + " " + src.Owner.User.Surname));

            CreateMap<Owner, OwnerPreviewDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.Pets, opt => opt.MapFrom(src => src.Pets.Select(p => p.Name)));

            CreateMap<Patient, UpdatePatientDto>().ReverseMap();
        }
    }
}
