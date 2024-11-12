using keasocial.Models;
using keasocial.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace keasocial.Repositories;

public class LectureRepository : ILectureRepository
{
    private readonly KeasocialDbContext _keasocialDbContext;

    public LectureRepository(KeasocialDbContext keasocialDbContext)
    {
        _keasocialDbContext = keasocialDbContext;
    }

    public async Task<Lecture> GetAsync(int id)
    {
        return await _keasocialDbContext.Lectures.FindAsync(id);
    }

    public async Task<List<Lecture>> GetAsync()
    {
        return await _keasocialDbContext.Lectures.ToListAsync();
    }
}