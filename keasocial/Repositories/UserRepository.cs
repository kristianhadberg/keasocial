using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using Neo4j.Driver;
using Neo4jClient;

namespace keasocial.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IGraphClient _graphClient;

    public UserRepository(IGraphClient graphClient)
    {
        _graphClient = graphClient;
    }

    /*
     * Perhaps ids should be removed from the user nodes entirely?
     */
    public async Task<User> GetAsync(string uuid)
    {
        var query = await _graphClient.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Uuid == uuid)
            .Return<User>("u")
            .ResultsAsync;
        
        return query.FirstOrDefault();
    }

    public async Task<List<User>> GetAsync()
    {
        var users = await _graphClient.Cypher
            .Match("(u:User)")
            .Return<User>("u")
            .ResultsAsync;

        return users.ToList();
    }

    public async Task<User> Create(User user)
    {
        var newUser = await _graphClient.Cypher
            .Create("(u:User {Uuid: randomUUID(), Name: $name, Email: $email, Password: $password})")
            .WithParams(new
            {
                name = user.Name,
                email = user.Email,
                password = user.Password
            })
            .Return<User>("u")
            .ResultsAsync;

        return newUser.FirstOrDefault();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _graphClient.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Email == email)
            .Return<User>("u")
            .ResultsAsync;

        return user.FirstOrDefault();
    }

    public async Task<User> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _graphClient.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Email == loginRequestDto.Email)
            .Return<User>("u")
            .ResultsAsync;

        return user.FirstOrDefault();
    }
    
}