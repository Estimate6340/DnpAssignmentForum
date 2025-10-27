using ApiContracts.DTOs;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDTO> AddAsync(CreatePostDTO create);
    Task<IReadOnlyList<PostDTO>> GetManyAsync(string? title = null, int? userId = null, string? username = null, bool includeComments = false);
    Task<PostDTO?> GetByIdAsync(int id, bool includeComments = false);
}