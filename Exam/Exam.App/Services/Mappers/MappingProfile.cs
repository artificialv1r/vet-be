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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
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
                    opt => opt.MapFrom(src => src.Owner.User.Name + " " + src.Owner.User.Surname))
                .ForMember(dest => dest.OwnerUsername, opt => opt.MapFrom(src => src.Owner.User.UserName))
                .ForMember(dest => dest.AnimalSpeciesId, opt => opt.MapFrom(src => src.AnimalSpeciesId));

            CreateMap<Owner, OwnerPreviewDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.Pets, opt => opt.MapFrom(src => src.Pets.Select(p => p.Name)));

            CreateMap<Patient, UpdatePatientDto>().ReverseMap();

            CreateMap<Vet, VetPreviewDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname));

            CreateMap<Examination, ExaminationPreviewDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.Name))
                .ForMember(dest => dest.AnimalSpecie, opt => opt.MapFrom(src => src.Patient.AnimalSpecies.Name))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Now.Year - src.Patient.DateOfBirth.Year))
                .ForMember(dest => dest.VetName, opt => opt.MapFrom(src => src.Vet.User.Name + " " + src.Vet.User
                    .Surname))
                .ForMember(dest => dest.Status, opt =>opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Vet, VetDetailsPreviewDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.Examinations, opt => opt.MapFrom(src => src.Examinations));

            CreateMap<ExamReport, CreateReportDto>().ReverseMap();
            CreateMap<ExamReport, UpdateReportDto>().ReverseMap();
        }
    }
}
