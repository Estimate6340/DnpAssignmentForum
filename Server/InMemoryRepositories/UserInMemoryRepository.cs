using Entities;
using RepositoryContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private int _nextId = 1;
    private readonly object _lock = new();

    public UserInMemoryRepository()
    {
        // seed
        AddAsync(new User { Username = "alice", Password = "pw1" }).Wait();
        AddAsync(new User { Username = "bob", Password = "pw2" }).Wait();
        AddAsync(new User { Username = "charlie", Password = "pw3" }).Wait();
    }

    public Task<User> AddAsync(User user)
    {
        lock (_lock)
        {
            user.Id = _nextId++;
            _users.Add(user);
        }
        return Task.FromResult(user);
    }

    public Task<IEnumerable<User>> GetAllAsync() => Task.FromResult(_users.AsEnumerable());

    public Task<User?> GetByIdAsync(int id) => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

    public Task<User?> GetByUsernameAsync(string username) =>
        Task.FromResult(_users.FirstOrDefault(u => u.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase)));

    public Task<User> UpdateAsync(User user)
    {
        lock (_lock)
        {
            var idx = _users.FindIndex(u => u.Id == user.Id);
            if (idx == -1) throw new KeyNotFoundException("User not found");
            _users[idx] = user;
        }
        return Task.FromResult(user);
    }

    public Task DeleteAsync(int id)
    {
        lock (_lock)
        {
            _users.RemoveAll(u => u.Id == id);
        }
        return Task.CompletedTask;
    }
}