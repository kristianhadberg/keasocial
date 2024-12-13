/*using keasocial.Data;
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

    public async Task<Lecture> Create(Lecture lecture)
    {
        await _keasocialDbContext.Lectures.AddAsync(lecture);
        await _keasocialDbContext.SaveChangesAsync();
        
        return lecture;
    }

    public async Task<Lecture> Update(int id, Lecture lecture)
    {
        var updatedLecture = _keasocialDbContext.Lectures.Update(lecture);
        await _keasocialDbContext.SaveChangesAsync();

        return updatedLecture.Entity;
    }

    public async Task<Lecture> Delete(int id)
    {
        var lecture = await _keasocialDbContext.Lectures.FindAsync(id);

        var deletedLecture = _keasocialDbContext.Lectures.Remove(lecture);
        await _keasocialDbContext.SaveChangesAsync();

        return deletedLecture.Entity;
    }
}*/