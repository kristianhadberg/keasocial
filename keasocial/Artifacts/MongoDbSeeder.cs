/*
namespace keasocial.Artifacts;

using MongoDB.Driver;
using keasocial.Models;

class MongoDbSeeder
{
    static async Task Main(string[] args)
    {
        var client = new MongoClient("mongodb://localhost:27017"); // Adjust connection string as needed
        var database = client.GetDatabase("keasocial");

        // Collections
        var usersCollection = database.GetCollection<User>("Users");
        var calendarsCollection = database.GetCollection<Calendar>("Calendars");
        var lecturesCollection = database.GetCollection<Lecture>("Lectures");
        var calendarLectureCollection = database.GetCollection<BsonDocument>("CalendarLecture");
        var postsCollection = database.GetCollection<Post>("Posts");
        var commentsCollection = database.GetCollection<Comment>("Comments");
        var postLikesCollection = database.GetCollection<PostLike>("PostLikes");
        var commentLikesCollection = database.GetCollection<CommentLike>("CommentLikes");

        // Clear existing data
        await usersCollection.DeleteManyAsync(_ => true);
        await calendarsCollection.DeleteManyAsync(_ => true);
        await lecturesCollection.DeleteManyAsync(_ => true);
        await calendarLectureCollection.DeleteManyAsync(_ => true);
        await postsCollection.DeleteManyAsync(_ => true);
        await commentsCollection.DeleteManyAsync(_ => true);
        await postLikesCollection.DeleteManyAsync(_ => true);
        await commentLikesCollection.DeleteManyAsync(_ => true);

        // Seed Users
        var users = new List<User>
        {
            new User { UserId = 1, Name = "John Doe", Email = "john@example.com", Password = "password123" },
            new User { UserId = 2, Name = "Jane Smith", Email = "jane@example.com", Password = "password123" },
            new User { UserId = 3, Name = "Alice Brown", Email = "alice@example.com", Password = "password123" },
            new User { UserId = 4, Name = "Bob Johnson", Email = "bob@example.com", Password = "password123" }
        };
        await usersCollection.InsertManyAsync(users);

        // Seed Calendars
        var calendars = new List<Calendar>
        {
            new Calendar { CalendarId = 1, UserId = 1, IsPublic = true },
            new Calendar { CalendarId = 2, UserId = 2, IsPublic = false },
            new Calendar { CalendarId = 3, UserId = 3, IsPublic = true },
            new Calendar { CalendarId = 4, UserId = 4, IsPublic = true }
        };
        await calendarsCollection.InsertManyAsync(calendars);

        // Seed Lectures
        var lectures = new List<Lecture>
        {
            new Lecture { LectureId = 1, LectureTitle = "Introduction to C#", LectureDescription = "A beginner-friendly introduction to C# and .NET.", LectureDate = DateTime.Parse("2024-09-21"), LectureTime = TimeSpan.Parse("10:00:00") },
            new Lecture { LectureId = 2, LectureTitle = "Advanced Database Design", LectureDescription = "Deep dive into database design principles.", LectureDate = DateTime.Parse("2024-09-22"), LectureTime = TimeSpan.Parse("14:00:00") },
            new Lecture { LectureId = 3, LectureTitle = "Game Development 101", LectureDescription = "Learn the basics of game development with Unity.", LectureDate = DateTime.Parse("2024-09-23"), LectureTime = TimeSpan.Parse("09:00:00") },
            new Lecture { LectureId = 4, LectureTitle = "Outdoor Survival Skills", LectureDescription = "Learn the essential skills for outdoor survival.", LectureDate = DateTime.Parse("2024-09-24"), LectureTime = TimeSpan.Parse("11:00:00") }
        };
        await lecturesCollection.InsertManyAsync(lectures);

        // Seed CalendarLectures
        var calendarLectures = new List<BsonDocument>
        {
            new BsonDocument { { "CalendarsCalendarId", 1 }, { "LecturesLectureId", 1 } },
            new BsonDocument { { "CalendarsCalendarId", 1 }, { "LecturesLectureId", 2 } },
            new BsonDocument { { "CalendarsCalendarId", 2 }, { "LecturesLectureId", 3 } },
            new BsonDocument { { "CalendarsCalendarId", 3 }, { "LecturesLectureId", 4 } },
            new BsonDocument { { "CalendarsCalendarId", 4 }, { "LecturesLectureId", 1 } }
        };
        await calendarLectureCollection.InsertManyAsync(calendarLectures);

        // Seed Posts
        var posts = new List<Post>
        {
            new Post { PostId = 1, Content = "Hello, World! This is my first post.", CreatedAt = DateTime.UtcNow, LikeCount = 0, UserId = 1 },
            new Post { PostId = 2, Content = "Just finished a great book on software architecture!", CreatedAt = DateTime.UtcNow, LikeCount = 0, UserId = 2 },
            new Post { PostId = 3, Content = "Anyone up for a hike this weekend?", CreatedAt = DateTime.UtcNow, LikeCount = 0, UserId = 3 },
            new Post { PostId = 4, Content = "Learning how to make video games is awesome!", CreatedAt = DateTime.UtcNow, LikeCount = 0, UserId = 4 }
        };
        await postsCollection.InsertManyAsync(posts);

        // Seed Comments
        var comments = new List<Comment>
        {
            new Comment { CommentId = "1", Content = "Welcome to the platform, John!", CreatedAt = DateTime.UtcNow, LikeCount = 0, PostId = 1, UserId = 2 },
            new Comment { CommentId = "2", Content = "What book did you read?", CreatedAt = DateTime.UtcNow, LikeCount = 0, PostId = 2, UserId = 3 },
            new Comment { CommentId = "3", Content = "Count me in for the hike!", CreatedAt = DateTime.UtcNow, LikeCount = 0, PostId = 3, UserId = 4 },
            new Comment { CommentId = "4", Content = "Can you share some game development resources?", CreatedAt = DateTime.UtcNow, LikeCount = 0, PostId = 4, UserId = 1 }
        };
        await commentsCollection.InsertManyAsync(comments);

        // Seed PostLikes
        var postLikes = new List<PostLike>
        {
            new PostLike { PostId = 1, UserId = 2 },
            new PostLike { PostId = 2, UserId = 3 },
            new PostLike { PostId = 3, UserId = 4 }
        };
        await postLikesCollection.InsertManyAsync(postLikes);

        // Seed CommentLikes
        var commentLikes = new List<CommentLike>
        {
            new CommentLike { CommentId = 1, UserId = 1 },
            new CommentLike { CommentId = 2, UserId = 2 },
            new CommentLike { CommentId = 3, UserId = 3 },
            new CommentLike { CommentId = 4, UserId = 4 }
        };
        await commentLikesCollection.InsertManyAsync(commentLikes);

        Console.WriteLine("Data seeding completed successfully.");
    }
}
*/
