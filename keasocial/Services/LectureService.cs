using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using keasocial.Services.Interfaces;

namespace keasocial.Services;


public class LectureService : ILectureService
{
    private readonly ILectureRepository _lectureRepository;

    public LectureService(ILectureRepository lectureRepository)
    {
        _lectureRepository = lectureRepository;
    }

    public async Task<Lecture> GetAsync(int id)
    {
        return await _lectureRepository.GetAsync(id);
    }

    public async Task<List<Lecture>> GetAsync()
    {
        return await _lectureRepository.GetAsync();
    }

    public async Task<Lecture> Create(LectureCreateDto lectureCreateDto)
    {
        var lecture = new Lecture
        {
            LectureTitle = lectureCreateDto.LectureTitle,
            LectureDescription = lectureCreateDto.LectureDescription,
            LectureDate = DateTime.SpecifyKind(lectureCreateDto.LectureDate, DateTimeKind.Utc),
            LectureTime = lectureCreateDto.LectureTime
        };
        
        return await _lectureRepository.Create(lecture);
    }

    public async Task<Lecture> Update(int id, LectureUpdateDto lectureUpdateDto)
    {
        var lecture = await _lectureRepository.GetAsync(id);

        lecture.LectureTitle = lectureUpdateDto.LectureTitle;
        lecture.LectureDescription = lectureUpdateDto.LectureDescription;
        lecture.LectureDate = DateTime.SpecifyKind(lectureUpdateDto.LectureDate, DateTimeKind.Utc);
        lecture.LectureTime = lectureUpdateDto.LectureTime;

        var updatedLecture = await _lectureRepository.Update(id, lecture);

        return updatedLecture;
    }

    public async Task<Lecture> Delete(int id)
    {
        return await _lectureRepository.Delete(id);
    }
}