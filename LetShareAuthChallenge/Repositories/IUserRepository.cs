using LetShareAuthChallenge.Models;
using System.Threading.Tasks;

namespace LetShareAuthChallenge.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
