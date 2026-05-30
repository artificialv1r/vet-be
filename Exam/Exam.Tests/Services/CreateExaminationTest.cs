using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Moq;

namespace Exam.Tests.Services;

public class CreateExaminationTest
{
    [Fact]
    public async Task Create_examination_should_succeed_vet_is_available()
    {
        // Arrange
        var examinationRepo = new Mock<IExaminationRepository>();
        var vetRepo = new Mock<IVetRepository>();
        var mapper = new Mock<IMapper>();

        var dto = new CreateExaminationDto
        {
            ExaminationDate = new DateTime(2026, 6, 1, 14, 30, 0),
            PatientId = 1,
            VetId = 1
        };

        var vet = new Vet
        {
            Id = 1,
            Patients = new List<Patient>
            {
                new Patient { Id = 1 }
            },
            Examinations = new List<Examination>()
        };

        vetRepo
            .Setup(r => r.GetVetById(vet.Id))
            .ReturnsAsync(vet);
        
        examinationRepo
            .Setup(r => r.Create(It.IsAny<Examination>()))
            .ReturnsAsync((Examination e) => e);
        
        var service = new ExaminationService(
            examinationRepo.Object,
            vetRepo.Object,
            mapper.Object
            );
        
        // Act
        await service.CreateExamination(dto);
        
        // Assert
        examinationRepo.Verify(r => r.Create(It.Is<Examination>(e => e.PatientId == dto.PatientId && e.VetId == 
                dto.VetId && e.Status == ExaminationStatus.Active)), 
            Times.Once);
    }

    [Fact]
    public async Task Create_examination_should_fail_vet_is_not_available()
    {
        // Arrange
        var examinationRepo = new Mock<IExaminationRepository>();
        var vetRepo = new Mock<IVetRepository>();
        var mapper = new Mock<IMapper>();

        var dto = new CreateExaminationDto
        {
            ExaminationDate = new DateTime(2026, 6, 1, 14, 30, 0),
            PatientId = 1,
            VetId = 1
        };

        var vet = new Vet
        {
            Id = 1,
            Patients = new List<Patient>
            {
                new Patient { Id = 1 }
            },
            Examinations = new List<Examination>
            {
                new Examination
                {
                    Id = 1,
                    ExaminationDate = new DateTime(2026,6,1,14,40,0),
                    Status = ExaminationStatus.Active
                }
            }
        };

        vetRepo
            .Setup(r => r.GetVetById(vet.Id))
            .ReturnsAsync(vet);

        var service = new ExaminationService(
            examinationRepo.Object,
            vetRepo.Object,
            mapper.Object
        );

        // Act
        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateExamination(dto));

        // Assert
        examinationRepo.Verify(r => r.Create(It.IsAny<Examination>()), Times.Never);
    }
    
}