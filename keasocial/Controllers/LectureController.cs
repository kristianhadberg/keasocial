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
    public async Task<ActionResult<Lecture>> Get()
    {
        var lectures = await _lectureService.GetAsync();
        return Ok(lectures);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<List<Lecture>>> Get(int id)
    {
        var lecture = await _lectureService.GetAsync(id);
        return Ok(lecture);
    }
    
}