using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryContracts;

public interface IPostRepository
{
    Task<Post> AddAsync(Post post);
    Task<IEnumerable<Post>> GetAllAsync();
    Task<Post?> GetByIdAsync(int id);
    Task<Post> UpdateAsync(Post post);
    Task DeleteAsync(int id);
    Task<IEnumerable<Post>> GetByUserIdAsync(int userId);
}