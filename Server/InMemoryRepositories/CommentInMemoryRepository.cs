using Entities;
using RepositoryContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private readonly List<Comment> _comments = new();
    private int _nextId = 1;
    private readonly object _lock = new();

    public CommentInMemoryRepository()
    {
        // seed: comment on post 1 by user 2, etc. Adjust if your seeded post/user ids differ.
        AddAsync(new Comment { PostId = 1, UserId = 2, Body = "Nice post!" }).Wait();
        AddAsync(new Comment { PostId = 1, UserId = 3, Body = "Welcome!" }).Wait();
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        lock (_lock)
        {
            comment.Id = _nextId++;
            _comments.Add(comment);
        }
        return Task.FromResult(comment);
    }

    public Task<IEnumerable<Comment>> GetAllAsync() => Task.FromResult(_comments.AsEnumerable());

    public Task<Comment?> GetByIdAsync(int id) => Task.FromResult(_comments.FirstOrDefault(c => c.Id == id));

    public Task<IEnumerable<Comment>> GetByPostIdAsync(int postId) => Task.FromResult(_comments.Where(c => c.PostId == postId));

    public Task<IEnumerable<Comment>> GetByUserIdAsync(int userId) => Task.FromResult(_comments.Where(c => c.UserId == userId));

    public Task<Comment> UpdateAsync(Comment comment)
    {
        lock (_lock)
        {
            var idx = _comments.FindIndex(c => c.Id == comment.Id);
            if (idx == -1) throw new KeyNotFoundException("Comment not found");
            _comments[idx] = comment;
        }
        return Task.FromResult(comment);
    }

    public Task DeleteAsync(int id)
    {
        lock (_lock)
        {
            _comments.RemoveAll(c => c.Id == id);
        }
        return Task.CompletedTask;
    }
}