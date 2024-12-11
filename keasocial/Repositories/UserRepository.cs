using keasocial.Dto;
using keasocial.Models;
using keasocial.Repositories.Interfaces;
using Neo4j.Driver;

namespace keasocial.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDriver _driver;

    public UserRepository(IDriver driver)
    {
        _driver = driver;
    }

    /*
     * Perhaps ids should be removed from the user nodes entirely?
     */
    public async Task<User> GetAsync(int id)
    {
        
        var (queryResults, _) = await _driver
            .ExecutableQuery(@"
                MATCH (u:User)
                WHERE u.UserId = $id
                RETURN u.UserId AS UserId, u.Name AS Name, u.Email AS Email")
            .WithParameters(new { id })
            .ExecuteAsync();

        var userResult = queryResults
            .Select(
                record => new User
                {
                    Name = record["Name"].As<String>(),
                    Email = record["Email"].As<String>(),
                })
            .FirstOrDefault();

        if (userResult == null)
        {
            return null;
        }

        return userResult;
    }

    public async Task<List<User>> GetAsync()
    {
        var (queryResults, _) = await _driver
            .ExecutableQuery(@"
                MATCH (u:User) RETURN u.UserId AS UserId, u.Name AS Name, u.Email AS Email")
            .ExecuteAsync();

        var userResult = queryResults
            .Select(
                record => new User
                {
                    Name = record["Name"].As<String>(),
                    Email = record["Email"].As<String>(),
                })
            .ToList();

        return userResult;
    }

    public async Task<User> Create(User user)
    {
        var (queryResults, _) = await _driver
            .ExecutableQuery(@"
               CREATE (u:User {Name: $name, Email: $email, Password: $password})
               RETURN u.Name AS Name, u.Email AS Email")
            .WithParameters(new { name = user.Name, email = user.Email, password = user.Password })
            .ExecuteAsync();

        return queryResults
            .Select(
                record => new User
                {
                    Name = record["Name"].As<String>(),
                    Email = record["Email"].As<String>()
                })
            .Single();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var (queryResults, _) = await _driver
            .ExecutableQuery(@"
                MATCH (u:User)
                WHERE u.Email = $email
                RETURN u.UserId AS UserId, u.Name AS Name, u.Email AS Email")
            .WithParameters(new { email })
            .ExecuteAsync();

        var userResult = queryResults
            .Select(
                record => new User
                {
                    Name = record["Name"].As<String>(),
                    Email = record["Email"].As<String>(),
                })
            .FirstOrDefault();

        if (userResult == null)
        {
            return null;
        }

        return userResult;
    }

    public async Task<User> Login(LoginRequestDto loginRequestDto)
    {
        var (queryResults, _) = await _driver
               .ExecutableQuery(@"
                   MATCH (u:User)
                   WHERE u.Email = $email
                   RETURN u.UserId AS UserId, u.Name AS Name, u.Email AS Email, u.Password AS Password")
               .WithParameters(new { email = loginRequestDto.Email })
               .ExecuteAsync();

           var userResult = queryResults
               .Select(
                   record => new User
                   {
                       Name = record["Name"].As<String>(),
                       Email = record["Email"].As<String>(),
                       Password = record["Password"].As<String>()
                   })
               .FirstOrDefault();

           if (userResult == null)
           {
               return null;
           }

           return userResult;
    }
    
}