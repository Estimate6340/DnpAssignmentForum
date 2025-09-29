using Entities;
using System.Text.Json;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true };

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson, jsonOptions) ?? new List<Post>();

        int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
        post.Id = maxId + 1;

        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts, jsonOptions);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson, jsonOptions) ?? new List<Post>();
        return posts;
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson, jsonOptions) ?? new List<Post>();
        return posts.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Post> UpdateAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson, jsonOptions) ?? new List<Post>();

        int idx = posts.FindIndex(p => p.Id == post.Id);
        if (idx == -1)
        {
            throw new InvalidOperationException($"Post with Id {post.Id} not found.");
        }

        posts[idx] = post;
        postsAsJson = JsonSerializer.Serialize(posts, jsonOptions);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
    }

    public async Task DeleteAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson, jsonOptions) ?? new List<Post>();

        int removed = posts.RemoveAll(p => p.Id == id);
        if (removed > 0)
        {
            postsAsJson = JsonSerializer.Serialize(posts, jsonOptions);
            await File.WriteAllTextAsync(filePath, postsAsJson);
        }
    }

    public async Task<IEnumerable<Post>> GetByUserIdAsync(int userId)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson, jsonOptions) ?? new List<Post>();
        return posts.Where(p => p.UserId == userId).ToList();
    }
}