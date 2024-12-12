using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace keasocial.Models;

public class Lecture
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public int LectureId { get; set; }
    public string LectureTitle { get; set; }
    public string LectureDescription { get; set; }
    public DateTime LectureDate { get; set; }
    public TimeSpan LectureTime { get; set; }

    // Navigation property
    public ICollection<Calendar> Calendars { get; set; }
}