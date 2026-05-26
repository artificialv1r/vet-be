using System.Security.Claims;
using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services;
using Exam.App.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ReportsController: ControllerBase
{
    private readonly IReportService _reportService;
    
    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [Authorize (Roles = "Vet")]
    [HttpPost]
    public Task<CreateReportDto> Create([FromBody] CreateReportDto reportDto)
    {
        string username = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return _reportService.Create(reportDto, username);
    }

    [Authorize (Roles = "Vet")]
    [HttpPut ("{id}")]
    public Task<UpdateReportDto> Update([FromBody] UpdateReportDto reportDto, int id)
    {
        return _reportService.Update(reportDto, id);
    }
}