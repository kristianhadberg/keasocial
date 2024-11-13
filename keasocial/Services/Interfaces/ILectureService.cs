using keasocial.Dto;
using keasocial.Models;

namespace keasocial.Services.Interfaces;

public interface ILectureService
{
    Task<Lecture> GetAsync(int id);
    Task<List<Lecture>> GetAsync();
    Task<Lecture> Create(LectureCreateDto lectureCreateDto);
    Task<Lecture> Update(int id, LectureUpdateDto lectureUpdateDto);
    Task<Lecture> Delete(int id);
}