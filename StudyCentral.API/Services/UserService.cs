using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IUserService
{
    Task<User> GetUserById(int id);
}

public class UserService : IUserService
{
    public Task<User> GetUserById(int id)
    {
        throw new NotImplementedException();
    }
}