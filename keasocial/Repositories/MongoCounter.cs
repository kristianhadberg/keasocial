using MongoDB.Driver;
using MongoDB.Bson;

namespace keasocial.Repositories;

public class MongoCounter
{
    private readonly IMongoCollection<BsonDocument> _counters;

    public MongoCounter(IMongoDatabase database)
    {
        _counters = database.GetCollection<BsonDocument>("Counters");
    }

    public async Task<int> GetNextSequenceValue(string counterName)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", counterName);
        var update = Builders<BsonDocument>.Update.Inc("seq", 1); // Increment the sequence
        var options = new FindOneAndUpdateOptions<BsonDocument>
        {
            IsUpsert = true, // Create if not exists
            ReturnDocument = ReturnDocument.After // Return updated document
        };

        var result = await _counters.FindOneAndUpdateAsync(filter, update, options);
        return result["seq"].AsInt32;
    }
    
    public async Task InitializeCounter(string counterName)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", counterName);
        var update = Builders<BsonDocument>.Update.SetOnInsert("seq", 1); // Set initial value to 1
        var options = new UpdateOptions { IsUpsert = true }; // Create if not exists

        await _counters.UpdateOneAsync(filter, update, options);
    }
}
