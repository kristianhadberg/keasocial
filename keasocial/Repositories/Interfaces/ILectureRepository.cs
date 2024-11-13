using keasocial.Models;

namespace keasocial.Repositories.Interfaces;

public interface ILectureRepository
{
    Task<Lecture> GetAsync(int id);
    Task<List<Lecture>> GetAsync();
    Task<Lecture> Create(Lecture lecture);
    Task<Lecture> Update(int id, Lecture lecture);
    Task<Lecture> Delete(int id);
}