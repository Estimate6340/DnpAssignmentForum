using Entities;
using RepositoryContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private readonly List<Post> _posts = new();
    private int _nextId = 1;
    private readonly object _lock = new();

    public PostInMemoryRepository()
    {
        AddAsync(new Post { UserId = 1, Title = "Welcome", Body = "First post from Alice" }).Wait();
        AddAsync(new Post { UserId = 2, Title = "Hello world", Body = "Bob says hi" }).Wait();
    }

    public Task<Post> AddAsync(Post post)
    {
        lock (_lock)
        {
            post.Id = _nextId++;
            _posts.Add(post);
        }
        return Task.FromResult(post);
    }

    public Task<IEnumerable<Post>> GetAllAsync() => Task.FromResult(_posts.AsEnumerable());

    public Task<Post?> GetByIdAsync(int id) => Task.FromResult(_posts.FirstOrDefault(p => p.Id == id));

    public Task<Post> UpdateAsync(Post post)
    {
        lock (_lock)
        {
            var idx = _posts.FindIndex(p => p.Id == post.Id);
            if (idx == -1) throw new KeyNotFoundException("Post not found");
            _posts[idx] = post;
        }
        return Task.FromResult(post);
    }

    public Task DeleteAsync(int id)
    {
        lock (_lock)
        {
            _posts.RemoveAll(p => p.Id == id);
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Post>> GetByUserIdAsync(int userId) => Task.FromResult(_posts.Where(p => p.UserId == userId));
}