using keasocial.Models;
using keasocial.Repositories.Interfaces;
using MongoDB.Driver;

namespace keasocial.Repositories;

public class MongoLectureRepository : ILectureRepository
{
    private readonly IMongoCollection<Lecture> _lectures;

    public MongoLectureRepository(IMongoDatabase database)
    {
        _lectures = database.GetCollection<Lecture>("Lectures");
    }

    public async Task<Lecture> GetAsync(int id)
    {
        return await _lectures.Find(l => l.LectureId == id).FirstOrDefaultAsync();
    }

    public async Task<List<Lecture>> GetAsync()
    {
        return await _lectures.Find(_ => true).ToListAsync();
    }

    public async Task<Lecture> Create(Lecture lecture)
    {
        await _lectures.InsertOneAsync(lecture);
        return lecture;
    }

    public async Task<Lecture> Update(int id, Lecture lecture)
    {
        var result = await _lectures.ReplaceOneAsync(l => l.LectureId == id, lecture);
        if (result.MatchedCount == 0)
        {
            throw new KeyNotFoundException($"Lecture with ID {id} not found.");
        }
        return lecture;
    }

    public async Task<Lecture> Delete(int id)
    {
        var result = await _lectures.FindOneAndDeleteAsync(l => l.LectureId == id);
        if (result == null)
        {
            throw new KeyNotFoundException($"Lecture with ID {id} not found.");
        }
        return result;
    }
}