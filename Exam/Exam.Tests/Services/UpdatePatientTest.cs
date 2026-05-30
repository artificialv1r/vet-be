using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Infrastructure.Database.Repositories;
using Exam.App.Services;
using Exam.App.Services.Dtos;
using Moq;

namespace Exam.Tests.Services;

public class UpdatePatientTest
{
    [Fact]
    public async Task Update_patient_should_not_change_owner_change()
    {
        // Arrange
        var patientRepo = new Mock<IPatientRepository>();
        var vetService = new Mock<IVetService>();
        var ownerService = new Mock<IOwnerService>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var mapper = new Mock<IMapper>();

        var existingPatient = new Patient
        {
            Id = 1,
            Name = "Rex",
            OwnerId = 1,
            DateOfBirth = new DateTime(2020, 1, 1)
        };

        var dto = new UpdatePatientDto
        {
            Name = "Max",
            DateOfBirth = new DateTime(2021, 1, 1),
            VetId = null
        };
        
        patientRepo.Setup(r => r.GetPatientById(existingPatient.Id)).ReturnsAsync(existingPatient);
        mapper.Setup(m => m.Map<UpdatePatientDto>(It.IsAny<Patient>())).Returns(dto);
        
        var service = new PatientService(
            unitOfWork.Object,
            patientRepo.Object,
            ownerService.Object,
            vetService.Object,
            mapper.Object
            );
        
        // Act
        await service.UpdatePatient(1, dto);
        
        // Assert
        Assert.Equal(1, existingPatient.OwnerId);
        
        patientRepo.Verify(r => r.UpdatePatient(It.Is<Patient>(p => p.OwnerId == 1)), Times.Once);
    }
}