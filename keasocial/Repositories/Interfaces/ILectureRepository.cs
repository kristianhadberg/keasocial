using keasocial.Models;

namespace keasocial.Repositories.Interfaces;

public interface ILectureRepository
{
    Task<Lecture> GetAsync(int id);
    Task<List<Lecture>> GetAsync();
}