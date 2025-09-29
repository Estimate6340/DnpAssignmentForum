using Entities;
using System.Text.Json;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true };

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson, jsonOptions) ?? new List<User>();

        int maxId = users.Count > 0 ? users.Max(u => u.Id) : 0;
        user.Id = maxId + 1;

        users.Add(user);
        usersAsJson = JsonSerializer.Serialize(users, jsonOptions);
        await File.WriteAllTextAsync(filePath, usersAsJson);
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson, jsonOptions) ?? new List<User>();
        return users;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson, jsonOptions) ?? new List<User>();
        return users.FirstOrDefault(u => u.Id == id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson, jsonOptions) ?? new List<User>();
        return users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<User> UpdateAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson, jsonOptions) ?? new List<User>();

        int idx = users.FindIndex(u => u.Id == user.Id);
        if (idx == -1)
        {
            throw new InvalidOperationException($"User with Id {user.Id} not found.");
        }

        users[idx] = user;
        usersAsJson = JsonSerializer.Serialize(users, jsonOptions);
        await File.WriteAllTextAsync(filePath, usersAsJson);
        return user;
    }

    public async Task DeleteAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson, jsonOptions) ?? new List<User>();

        int removed = users.RemoveAll(u => u.Id == id);
        if (removed > 0)
        {
            usersAsJson = JsonSerializer.Serialize(users, jsonOptions);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }
    }
}