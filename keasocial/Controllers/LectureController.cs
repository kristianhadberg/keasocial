using keasocial.Dto;
using keasocial.Models;
using keasocial.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Lecture>> Post([FromBody]LectureCreateDto lectureCreateDto)
    {
        var newLecture = await _lectureService.Create(lectureCreateDto);
        return CreatedAtAction(nameof(Get), new { id = newLecture.LectureId }, newLecture);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Lecture>> Put(int id, [FromBody]LectureUpdateDto lectureUpdateDto)
    {
        var updatedLecture = await _lectureService.Update(id, lectureUpdateDto);

        return Ok(updatedLecture);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Lecture>> Delete(int id)
    {
        var deletedLecture = await _lectureService.Delete(id);

        return Ok(deletedLecture);
    }
    
}