using ApiContracts.DTOs;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<IReadOnlyList<CommentDTO>> GetForPostAsync(int postId);
    Task<CommentDTO> AddAsync(CreateCommentDTO create);
}