using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Task<Comment> AddAsync(Comment comment);
    Task<IEnumerable<Comment>> GetAllAsync();
    Task<Comment?> GetByIdAsync(int id);
    Task<IEnumerable<Comment>> GetByPostIdAsync(int postId);
    Task<IEnumerable<Comment>> GetByUserIdAsync(int userId);
    Task<Comment> UpdateAsync(Comment comment);
    Task DeleteAsync(int id);
}