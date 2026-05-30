using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services;
using Exam.App.Services.Dtos;
using Moq;

namespace Exam.Tests.Services;

public class CreateReportTest
{
    // Podnošenje izveštaja (3.3), gde testovi proveravaju uspešne i neuspešne slučajeve vezane za pripadnost pregleda veterinaru, status pregleda i vreme kada se može menjati izvešta

    [Fact]
    public async Task Create_report_should_succeed_when_vet_owns_examination()
    {
        // Arrange
        var reportRepo = new Mock<IReportRepository>();
        var examinationRepo = new Mock<IExaminationRepository>();
        var vetRepo = new Mock<IVetRepository>();
        var mapper = new Mock<IMapper>();

        var dto = new CreateReportDto
        {
            ExaminationId = 1,
            Description = "Sve je u redu",
            PatientWeight = 12.0
        };

        var examination = new Examination
        {
            Id = 1,
            ExaminationDate = new DateTime(2026, 6, 1, 14, 30, 0),
            Status = ExaminationStatus.Active,
            VetId = 1,
        };

        var mappedReport = new ExamReport
        {
            ExaminationId = dto.ExaminationId,
            Description = dto.Description,
            PatientWeight = dto.PatientWeight,
        };

        var vet = new Vet
        {
            Id = 1,
            User = new ApplicationUser
            {
                UserName = "daca",
                Name = "Danilo",
                Surname = "Lazic"
            }
        };
        
        examinationRepo
            .Setup(r => r.Get(examination.Id))
            .ReturnsAsync(examination);

        vetRepo
            .Setup(r => r.GetByUsername("daca"))
            .ReturnsAsync(vet);
        
        mapper
            .Setup(m => m.Map<ExamReport>(dto))
            .Returns(mappedReport);
        
        reportRepo
            .Setup(r => r.Create(It.IsAny<ExamReport>()))
            .ReturnsAsync(mappedReport);
        
        var service = new ReportService(
            reportRepo.Object,
            examinationRepo.Object,
            vetRepo.Object,
            mapper.Object
            );
        
        // Act
        await service.Create(dto, "daca" );
        
        // Assert
        reportRepo.Verify(
            r => r.Create(
                It.Is<ExamReport>(e => 
                    e.ExaminationId == dto.ExaminationId && 
                    e.PatientWeight == dto.PatientWeight)), 
            Times.Once);
    }
    
    [Fact]
    public async Task Create_report_should_fail_when_vet_does_not_own_examination()
    {
        // Arrange
        var reportRepo = new Mock<IReportRepository>();
        var examinationRepo = new Mock<IExaminationRepository>();
        var vetRepo = new Mock<IVetRepository>();
        var mapper = new Mock<IMapper>();

        var dto = new CreateReportDto
        {
            ExaminationId = 1,
            Description = "Sve je u redu",
            PatientWeight = 12.0
        };

        var examination = new Examination
        {
            Id = 1,
            ExaminationDate = new DateTime(2026, 6, 1, 14, 30, 0),
            Status = ExaminationStatus.Active,
            VetId = 1,
        };

        var vet = new Vet
        {
            Id = 2,
            User = new ApplicationUser
            {
                UserName = "mare",
                Name = "Marko",
                Surname = "Savic"
            }
        };
        
        examinationRepo
            .Setup(r => r.Get(examination.Id))
            .ReturnsAsync(examination);

        vetRepo
            .Setup(r => r.GetByUsername("mare"))
            .ReturnsAsync(vet);
        
        var service = new ReportService(
            reportRepo.Object,
            examinationRepo.Object,
            vetRepo.Object,
            mapper.Object
            );
        
        // Act
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.Create(dto,"mare"));
        
        // Assert
        reportRepo.Verify(
            r => r.Create(
                It.IsAny<ExamReport>()), 
            Times.Never);
    }

    [Fact]
    public async Task Update_report_should_succeed_when_deadline_is_active()
    {
        // Arrange
        var reportRepo = new Mock<IReportRepository>();
        var examinationRepo = new Mock<IExaminationRepository>();
        var vetRepo = new Mock<IVetRepository>();
        var mapper = new Mock<IMapper>();

        var dto = new UpdateReportDto
        {
            Description = "Sve je u redu",
            PatientWeight = 12.0
        };

        var existingReport = new ExamReport
        {
            Id = 1,
            ExaminationId = 1,
            Description = "Sve ..",
            PatientWeight = 13.0,
            CreatedAt = DateTime.UtcNow
        };

        var exam = new Examination
        {
            Id = 1,
            Status = ExaminationStatus.Active
        };

        reportRepo
            .Setup(r => r.Get(existingReport.Id))
            .ReturnsAsync(existingReport);

        examinationRepo
            .Setup(e => e.Get(exam.Id))
            .ReturnsAsync(exam);
        
        reportRepo
            .Setup(r => r.Update(It.IsAny<ExamReport>()))
            .ReturnsAsync((ExamReport e) => e);

        var service = new ReportService(
            reportRepo.Object,
            examinationRepo.Object,
            vetRepo.Object,
            mapper.Object);
        
        // Act
        await service.Update(dto, existingReport.Id);
        
        // Assert
        reportRepo.Verify(r => 
            r.Update(It.Is<ExamReport>(e => 
                e.Description == dto.Description &&
                e.PatientWeight == dto.PatientWeight)), 
            Times.Once);
    }

    [Fact]
    public async Task Update_report_should_fail_when_deadline_is_inactive()
    {
        // Arrange
        var reportRepo = new Mock<IReportRepository>();
        var examinationRepo = new Mock<IExaminationRepository>();
        var vetRepo = new Mock<IVetRepository>();
        var mapper = new Mock<IMapper>();

        var dto = new UpdateReportDto
        {
            Description = "Sve je u redu",
            PatientWeight = 12.0
        };

        var existingReport = new ExamReport
        {
            Id = 1,
            ExaminationId = 1,
            Description = "Sve ..",
            PatientWeight = 13.0,
            CreatedAt = DateTime.UtcNow.AddDays(-10)
        };

        reportRepo
            .Setup(r => r.Get(existingReport.Id))
            .ReturnsAsync(existingReport);

        var service = new ReportService(
            reportRepo.Object,
            examinationRepo.Object,
            vetRepo.Object,
            mapper.Object);

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.Update
            (dto,existingReport.Id));

        // Assert
        reportRepo.Verify(
            r => r.Update(
                It.IsAny<ExamReport>()),
            Times.Never);
    }
}