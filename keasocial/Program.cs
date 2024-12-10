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
    // Configure MySQL
    builder.Services.AddDbContext<KeasocialDbContext>(options =>
    {
        options.UseMySql(builder.Configuration.GetConnectionString("MySql"), new MySqlServerVersion(new Version(8, 0, 27)));
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

// Enable EF Core logging
var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddConsole(); // Log EF Core queries to the console
});

builder.Services.AddDbContext<KeasocialDbContext>(options =>
{
    options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"), 
            new MySqlServerVersion(new Version(9, 1, 0))
        )
        .UseLoggerFactory(loggerFactory)          // Attach logging factory
        .EnableSensitiveDataLogging()             // Show parameter values in logs
        .EnableDetailedErrors();                  // Provide detailed error messages
});

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