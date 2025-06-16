using LetShareAuthChallenge.Models;

namespace LetShareAuthChallenge.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
