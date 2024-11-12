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
}