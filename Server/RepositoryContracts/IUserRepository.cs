using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(int id);
}