using keasocial.Models;
using keasocial.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace keasocial.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LectureController : ControllerBase
{
    private readonly ILectureService _lectureService;

    public LectureController(ILectureService lectureService)
    {
        _lectureService = lectureService;
    }

    [HttpGet]
    public async Task<List<Lecture>> Get()
    {
        return await _lectureService.GetAsync();
    }

    [HttpGet("/{id}")]
    public async Task<Lecture> Get(int id)
    {
        return await _lectureService.GetAsync(id);
    }
    
}