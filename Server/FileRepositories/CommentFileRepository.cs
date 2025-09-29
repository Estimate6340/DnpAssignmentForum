using Entities;
using System.Text.Json;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";
    private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true };

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson, jsonOptions) ?? new List<Comment>();

        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
        comment.Id = maxId + 1;

        comments.Add(comment);
        commentsAsJson = JsonSerializer.Serialize(comments, jsonOptions);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
        return comment;
    }

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson, jsonOptions) ?? new List<Comment>();
        return comments;
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson, jsonOptions) ?? new List<Comment>();
        return comments.FirstOrDefault(c => c.Id == id);
    }

    public async Task<IEnumerable<Comment>> GetByPostIdAsync(int postId)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson, jsonOptions) ?? new List<Comment>();
        return comments.Where(c => c.PostId == postId).ToList();
    }

    public async Task<IEnumerable<Comment>> GetByUserIdAsync(int userId)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson, jsonOptions) ?? new List<Comment>();
        return comments.Where(c => c.UserId == userId).ToList();
    }

    public async Task<Comment> UpdateAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson, jsonOptions) ?? new List<Comment>();

        int idx = comments.FindIndex(c => c.Id == comment.Id);
        if (idx == -1)
        {
            throw new InvalidOperationException($"Comment with Id {comment.Id} not found.");
        }

        comments[idx] = comment;
        commentsAsJson = JsonSerializer.Serialize(comments, jsonOptions);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
        return comment;
    }

    public async Task DeleteAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson, jsonOptions) ?? new List<Comment>();

        int removed = comments.RemoveAll(c => c.Id == id);
        if (removed > 0)
        {
            commentsAsJson = JsonSerializer.Serialize(comments, jsonOptions);
            await File.WriteAllTextAsync(filePath, commentsAsJson);
        }
    }
}