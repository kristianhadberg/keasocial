using System.Text;
using keasocial.ErrorHandling;
using keasocial.Repositories;
using keasocial.Repositories.Interfaces;
using keasocial.Security;
using keasocial.Services;
using keasocial.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Neo4jClient;

var builder = WebApplication.CreateBuilder(args);

/*
 * Services & Repositories
 */
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
/*builder.Services.AddScoped<ILectureService, LectureService>();*/
builder.Services.AddScoped<IPostService, PostService>();
/*builder.Services.AddScoped<ICommentService, CommentService>();*/

builder.Services.AddScoped<IUserRepository, UserRepository>();
/*builder.Services.AddScoped<ILectureRepository, LectureRepository>();*/
builder.Services.AddScoped<IPostRepository, PostRepository>();
/*builder.Services.AddScoped<ICommentRepository, CommentRepository>();*/
builder.Services.AddSingleton<JwtService>();

builder.Services.AddSingleton<IGraphClient>(provider =>
{
    var uri = builder.Configuration["Neo4j:Uri"];
    var username = builder.Configuration["Neo4j:Username"];
    var password = builder.Configuration["Neo4j:Password"];
    var client = new BoltGraphClient(uri, username, password);
    client.ConnectAsync().Wait();
    return client;
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