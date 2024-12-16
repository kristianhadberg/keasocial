using System.Configuration;
using System.Text;
using keasocial.Data;
using keasocial.ErrorHandling;
using keasocial.Models;
using keasocial.Repositories;
using keasocial.Repositories.Interfaces;
using keasocial.Security;
using keasocial.Services;
using keasocial.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using MongoDB.Driver;

using MongoDB.Driver;
using keasocial.Models;
using System.Collections.Generic;
using MongoDB.Bson;

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
    new CommentLike { CommentId = "1", UserId = 1 },
    new CommentLike { CommentId = "2", UserId = 2 },
    new CommentLike { CommentId = "3", UserId = 3 },
    new CommentLike { CommentId = "4", UserId = 4 }
};
await commentLikesCollection.InsertManyAsync(commentLikes);

Console.WriteLine("Data seeding completed successfully.");
Console.WriteLine("MongoDB seeding completed successfully.");


var builder = WebApplication.CreateBuilder(args);
var useMongoDb = builder.Configuration.GetValue<bool>("DatabaseConfig:UseMongoDB");

if (useMongoDb)
{
    // Configure MongoDB
    var mongoConnectionString = builder.Configuration.GetSection("ConnectionStrings:MongoDB:ConnectionString").Value;
    var mongoDatabaseName = builder.Configuration.GetSection("ConnectionStrings:MongoDB:DatabaseName").Value;

    // Register MongoDB services
    builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(mongoConnectionString));
    builder.Services.AddSingleton(s =>
    {
        var client = s.GetRequiredService<IMongoClient>();
        return client.GetDatabase(mongoDatabaseName);
    });

    // Register MongoDB repositories
    builder.Services.AddScoped<IUserRepository, MongoUserRepository>();
    builder.Services.AddScoped<ILectureRepository, MongoLectureRepository>();
    builder.Services.AddScoped<ICommentRepository, MongoCommentRepository>();
    builder.Services.AddScoped<IPostRepository, MongoPostRepository>();
}
else
{
    var loggerFactory = LoggerFactory.Create(loggingBuilder =>
    {
        loggingBuilder.AddConsole(); // Log EF Core queries to the console
    });
    // Configure MySQL
    builder.Services.AddDbContext<KeasocialDbContext>(options =>
    {
        options.UseMySql(
            builder.Configuration.GetConnectionString("MySql"),
            new MySqlServerVersion(new Version(8, 0, 27))
        )
        .UseLoggerFactory(loggerFactory)          // Attach logging factory
        .EnableSensitiveDataLogging()             // Show parameter values in logs
        .EnableDetailedErrors();      
        
    });

    // Register MySQL repositories
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ILectureRepository, LectureRepository>();
    builder.Services.AddScoped<ICommentRepository, CommentRepository>();
    builder.Services.AddScoped<IPostRepository, PostRepository>();
}

/*
 * Services & Repositories
 */
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILectureService, LectureService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddSingleton<JwtService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

/*
 * JWT
 */
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();




app.Run();