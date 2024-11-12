using keasocial.Models;

namespace keasocial.Services.Interfaces;

public interface ILectureService
{
    Task<Lecture> GetAsync(int id);
    Task<List<Lecture>> GetAsync();
    
}