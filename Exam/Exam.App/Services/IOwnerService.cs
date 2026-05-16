using Exam.App.Services.Dtos;

namespace Exam.App.Services;

public interface IOwnerService
{
    Task<OwnerPreviewDto> FindByUsernameAsync(string username);
}