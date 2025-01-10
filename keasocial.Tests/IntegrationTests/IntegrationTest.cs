
using System.Net.Http.Headers;
using System.Net.Http.Json;
using keasocial.Data;
using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories;
using keasocial.Repositories.Interfaces;
using keasocial.Services;
using keasocial.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace keasocial.Tests.IntegrationTests;

public class IntegrationTestSetup
{
    protected readonly HttpClient TestClient;
    
    protected IntegrationTestSetup()
    {
        /*var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove all existing DbContext registrations
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<KeasocialDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Register the in-memory database
                    services.AddDbContext<KeasocialDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb"));

                    // Apply migrations or seed data if necessary
                    var serviceProvider = services.BuildServiceProvider();
                    using var scope = serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<KeasocialDbContext>();
                    context.Database.EnsureCreated();
                });
            });*/
        
        var TestDatabaseName = $"TestDb_{Guid.NewGuid()}";

        /*var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Testing"); // Or "Testing"
                builder.ConfigureServices(services =>
                {
                    // Remove all existing DbContext registrations
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<KeasocialDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Register the in-memory database with the test-specific name
                    services.AddDbContext<KeasocialDbContext>(options =>
                        options.UseInMemoryDatabase(TestDatabaseName)); // Shared for the current test instance

                    // Ensure the database is created
                    var serviceProvider = services.BuildServiceProvider();
                    using var scope = serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<KeasocialDbContext>();
                    context.Database.EnsureCreated();
                    Console.WriteLine("Database schema created successfully");
                });
            });*/
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Testing");
                builder.ConfigureServices(services =>
                {
                    // Remove DbContext registration and use the in-memory database
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<KeasocialDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<KeasocialDbContext>(options =>
                        options.UseInMemoryDatabase(TestDatabaseName));


                    services.RemoveAll<IPostService>();
                    services.RemoveAll<IUserService>();
                    services.RemoveAll<ICommentService>();
                    services.RemoveAll<LectureService>();
                    
                    
                    services.AddControllers();
                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<ILectureService, LectureService>();
                    services.AddScoped<IPostService, PostService>();
                    services.AddScoped<ICommentService, CommentService>();

                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<ILectureRepository, LectureRepository>();
                    services.AddScoped<IPostRepository, PostRepository>();
                    services.AddScoped<ICommentRepository, CommentRepository>();

                    // Build the service provider to apply changes
                    var serviceProvider = services.BuildServiceProvider();

                    // Validate the database and seed data if necessary
                    using var scope = serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<KeasocialDbContext>();
                    context.Database.EnsureCreated();

                });
            });
        
        TestClient = appFactory.CreateClient();
    }
    
    protected async Task<Post> CreatePostAsync(PostCreateDto postCreateDto)
    {
        var response = await TestClient.PostAsJsonAsync("api/Post", postCreateDto);
        return await response.Content.ReadAsAsync<Post>();
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    private async Task<string> GetJwtAsync()
    {
        var newUser = new UserCreateDto
        {
            Name = "Johnny",
            Email = "john@example.com",
            Password = "validpassword123"
        };

        var loginRequest = new LoginRequestDto
        {
            Email = newUser.Email,
            Password = newUser.Password
        };
        
        // Register the user
        await TestClient.PostAsJsonAsync("api/User/register", newUser);

        var response = await TestClient.PostAsJsonAsync("api/User/login", loginRequest);

        var loginResponse = await response.Content.ReadAsAsync<LoginResponseDto>();

        return loginResponse.Token;
    }
}